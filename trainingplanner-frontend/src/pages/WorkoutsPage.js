import { useEffect, useState } from 'react';

// API-adressen (från .env, med backup)
const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:7296';

function WorkoutsPage() {
  // ----- State för workouts-listan -----
  const [workouts, setWorkouts] = useState([]);
  const [loadingWorkouts, setLoadingWorkouts] = useState(true);
  const [workoutsError, setWorkoutsError] = useState('');

  // ----- State för users (till dropdown) -----
  const [users, setUsers] = useState([]);
  const [loadingUsers, setLoadingUsers] = useState(true);
  const [usersError, setUsersError] = useState('');

  // ----- State för exercises (för koppling) -----
  const [exercises, setExercises] = useState([]);
  const [loadingExercises, setLoadingExercises] = useState(true);
  const [exercisesError, setExercisesError] = useState('');

  // ----- State för "skapa workout"-formuläret -----
  const [title, setTitle] = useState('');
  const [date, setDate] = useState('');     // "YYYY-MM-DD" från <input type="date">
  const [userId, setUserId] = useState('');
  const [formError, setFormError] = useState('');
  const [formSuccess, setFormSuccess] = useState('');
  const [submittingWorkout, setSubmittingWorkout] = useState(false);

  // ----- State för "koppla exercise till workout"-formuläret -----
  const [selectedWorkoutId, setSelectedWorkoutId] = useState('');
  const [selectedExerciseId, setSelectedExerciseId] = useState('');
  const [sets, setSets] = useState('');
  const [reps, setReps] = useState('');
  const [linkError, setLinkError] = useState('');
  const [linkSuccess, setLinkSuccess] = useState('');
  const [submittingLink, setSubmittingLink] = useState(false);

  // ---------- Hämta workouts ----------
  const fetchWorkouts = async () => {
    try {
      setLoadingWorkouts(true);
      setWorkoutsError('');

      const url = `${API_URL}/api/workout`;
      console.log('Hämtar workouts från:', url);

      const response = await fetch(url);

      if (!response.ok) {
        const text = await response.text();
        throw new Error(`API-svar (workouts): ${response.status} – ${text}`);
      }

      const contentType = response.headers.get('content-type') || '';
      if (!contentType.includes('application/json')) {
        const text = await response.text();
        throw new Error('Workouts-API returnerade inte JSON: ' + text.slice(0, 100));
      }

      const data = await response.json();
      setWorkouts(data);
    }
    catch (err) {
      console.error('Fel när workouts hämtades:', err);
      setWorkoutsError(err.message || 'Ett okänt fel inträffade.');
    }
    finally {
      setLoadingWorkouts(false);
    }
  };

  // ---------- Hämta users ----------
  const fetchUsers = async () => {
    try {
      setLoadingUsers(true);
      setUsersError('');

      const url = `${API_URL}/api/user`;
      console.log('Hämtar users (för workouts) från:', url);

      const response = await fetch(url);

      if (!response.ok) {
        const text = await response.text();
        throw new Error(`API-svar (users): ${response.status} – ${text}`);
      }

      const contentType = response.headers.get('content-type') || '';
      if (!contentType.includes('application/json')) {
        const text = await response.text();
        throw new Error('Users-API returnerade inte JSON: ' + text.slice(0, 100));
      }

      const data = await response.json();
      setUsers(data);
    }
    catch (err) {
      console.error('Fel när users hämtades (för workouts):', err);
      setUsersError(err.message || 'Kunde inte ladda users.');
    }
    finally {
      setLoadingUsers(false);
    }
  };

  // ---------- Hämta exercises ----------
  const fetchExercises = async () => {
    try {
      setLoadingExercises(true);
      setExercisesError('');

      const url = `${API_URL}/api/exercise`;
      console.log('Hämtar exercises (för koppling) från:', url);

      const response = await fetch(url);

      if (!response.ok) {
        const text = await response.text();
        throw new Error(`API-svar (exercises): ${response.status} – ${text}`);
      }

      const contentType = response.headers.get('content-type') || '';
      if (!contentType.includes('application/json')) {
        const text = await response.text();
        throw new Error('Exercises-API returnerade inte JSON: ' + text.slice(0, 100));
      }

      const data = await response.json();
      setExercises(data);
    }
    catch (err) {
      console.error('Fel när exercises hämtades:', err);
      setExercisesError(err.message || 'Kunde inte ladda exercises.');
    }
    finally {
      setLoadingExercises(false);
    }
  };

  // Körs en gång när sidan laddas
  useEffect(() => {
    fetchWorkouts();
    fetchUsers();
    fetchExercises();
  }, []);

  // ---------- Skapa ny workout ----------
  const handleCreateWorkout = async (e) => {
    e.preventDefault();
    setFormError('');
    setFormSuccess('');

    if (!title.trim() || !date.trim() || !userId) {
      setFormError('Titel, datum och användare måste fyllas i.');
      return;
    }

    try {
      setSubmittingWorkout(true);

      const url = `${API_URL}/api/workout`;
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          title: title,
          date: date,
          userId: Number(userId)
        })
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(`Misslyckades att skapa workout: ${response.status} – ${text}`);
      }

      setFormSuccess('Workout skapad!');
      setTitle('');
      setDate('');
      setUserId('');

      await fetchWorkouts();
    }
    catch (err) {
      console.error('Fel vid skapande av workout:', err);
      setFormError(err.message || 'Ett okänt fel inträffade när workout skulle skapas.');
    }
    finally {
      setSubmittingWorkout(false);
    }
  };

  // ---------- Koppla exercise till workout ----------
  const handleAddExerciseToWorkout = async (e) => {
    e.preventDefault();
    setLinkError('');
    setLinkSuccess('');

    if (!selectedWorkoutId || !selectedExerciseId || !sets || !reps) {
      setLinkError('Välj workout, övning och ange both sets och reps.');
      return;
    }

    if (Number(sets) <= 0 || Number(reps) <= 0) {
      setLinkError('Sets och reps måste vara större än 0.');
      return;
    }

    try {
      setSubmittingLink(true);

      // OBS: om din backend-route heter annorlunda, t.ex.
      // `/api/workout/${id}/exercises` eller `/api/workout/${id}/add-exercise`
      // ändra raden nedan så den matchar din Swagger.
      const url = `${API_URL}/api/workout/${selectedWorkoutId}/exercise`;

      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          exerciseId: Number(selectedExerciseId),
          sets: Number(sets),
          reps: Number(reps)
        })
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(`Misslyckades att koppla exercise: ${response.status} – ${text}`);
      }

      setLinkSuccess('Övning kopplad till workout!');
      setSelectedWorkoutId('');
      setSelectedExerciseId('');
      setSets('');
      setReps('');
    }
    catch (err) {
      console.error('Fel vid koppling av exercise till workout:', err);
      setLinkError(err.message || 'Ett okänt fel inträffade när övningen skulle kopplas.');
    }
    finally {
      setSubmittingLink(false);
    }
  };

  // ---------- Render ----------

  return (
    <div>
      <h2>Workouts</h2>

      {/* FORMULÄR: SKAPA WORKOUT */}
      <h3>Skapa nytt träningspass</h3>
      <form onSubmit={handleCreateWorkout} style={{ marginBottom: '1rem' }}>
        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Titel:&nbsp;
            <input
              type="text"
              value={title}
              onChange={e => setTitle(e.target.value)}
            />
          </label>
        </div>

        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Datum:&nbsp;
            <input
              type="date"
              value={date}
              onChange={e => setDate(e.target.value)}
            />
          </label>
        </div>

        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Användare:&nbsp;
            {loadingUsers ? (
              <span>Laddar users...</span>
            ) : usersError ? (
              <span style={{ color: 'red' }}>Kunde inte ladda users.</span>
            ) : (
              <select
                value={userId}
                onChange={e => setUserId(e.target.value)}
              >
                <option value="">-- välj användare --</option>
                {users.map(user => (
                  <option key={user.id} value={user.id}>
                    {user.name} ({user.email})
                  </option>
                ))}
              </select>
            )}
          </label>
        </div>

        <button type="submit" disabled={submittingWorkout || loadingUsers}>
          {submittingWorkout ? 'Sparar...' : 'Spara workout'}
        </button>
      </form>

      {formError && <p style={{ color: 'red' }}>{formError}</p>}
      {formSuccess && <p style={{ color: 'green' }}>{formSuccess}</p>}

      <hr />

      {/* FORMULÄR: KOPPLA ÖVNING TILL WORKOUT */}
      <h3>Lägg till övning på ett workout</h3>
      <form onSubmit={handleAddExerciseToWorkout} style={{ marginBottom: '1rem' }}>
        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Workout:&nbsp;
            {loadingWorkouts ? (
              <span>Laddar workouts...</span>
            ) : workoutsError ? (
              <span style={{ color: 'red' }}>Kunde inte ladda workouts.</span>
            ) : (
              <select
                value={selectedWorkoutId}
                onChange={e => setSelectedWorkoutId(e.target.value)}
              >
                <option value="">-- välj workout --</option>
                {workouts.map(w => (
                  <option key={w.id} value={w.id}>
                    {w.title} ({w.date})
                  </option>
                ))}
              </select>
            )}
          </label>
        </div>

        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Övning:&nbsp;
            {loadingExercises ? (
              <span>Laddar övningar...</span>
            ) : exercisesError ? (
              <span style={{ color: 'red' }}>Kunde inte ladda övningar.</span>
            ) : (
              <select
                value={selectedExerciseId}
                onChange={e => setSelectedExerciseId(e.target.value)}
              >
                <option value="">-- välj övning --</option>
                {exercises.map(ex => (
                  <option key={ex.id} value={ex.id}>
                    {ex.name} ({ex.muscleGroup})
                  </option>
                ))}
              </select>
            )}
          </label>
        </div>

        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Sets:&nbsp;
            <input
              type="number"
              min="1"
              value={sets}
              onChange={e => setSets(e.target.value)}
              style={{ width: '60px' }}
            />
          </label>
        </div>

        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Reps:&nbsp;
            <input
              type="number"
              min="1"
              value={reps}
              onChange={e => setReps(e.target.value)}
              style={{ width: '60px' }}
            />
          </label>
        </div>

        <button type="submit" disabled={submittingLink || loadingWorkouts || loadingExercises}>
          {submittingLink ? 'Sparar...' : 'Lägg till övning på workout'}
        </button>
      </form>

      {linkError && <p style={{ color: 'red' }}>{linkError}</p>}
      {linkSuccess && <p style={{ color: 'green' }}>{linkSuccess}</p>}

      <hr />

      {/* LISTA MED WORKOUTS */}
      <h3>Befintliga workouts</h3>

      {loadingWorkouts ? (
        <p>Loading workouts...</p>
      ) : workoutsError ? (
        <p style={{ color: 'red' }}>Error: {workoutsError}</p>
      ) : workouts.length === 0 ? (
        <p>Inga workouts hittades.</p>
      ) : (
        <ul>
          {workouts.map(workout => {
            const user = users.find(u => u.id === workout.userId);
            const userName = user ? user.name : `UserId: ${workout.userId}`;

            return (
              <li key={workout.id}>
                <strong>{workout.title}</strong> – {workout.date?.toString?.() || workout.date}{' '}
                ({userName})
              </li>
            );
          })}
        </ul>
      )}
    </div>
  );
}

export default WorkoutsPage;
