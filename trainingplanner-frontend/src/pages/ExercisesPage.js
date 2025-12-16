import { useEffect, useState } from 'react';

// API-adressen (från .env, med backup)
const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:7296';

function ExercisesPage() {
  // ----- State för listan -----
  const [exercises, setExercises] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState('');

  // ----- State för formuläret -----
  const [name, setName] = useState('');
  const [muscleGroup, setMuscleGroup] = useState('');
  const [formError, setFormError] = useState('');
  const [formSuccess, setFormSuccess] = useState('');
  const [submitting, setSubmitting] = useState(false);

  // ---------- Hämta exercises från API ----------
  const fetchExercises = async () => {
    try {
      setLoading(true);
      setLoadError('');

      const url = `${API_URL}/api/exercise`;
      console.log('Hämtar exercises från:', url);

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
      setLoadError(err.message || 'Ett okänt fel inträffade.');
    }
    finally {
      setLoading(false);
    }
  };

  // Körs en gång när sidan laddas
  useEffect(() => {
    fetchExercises();
  }, []);

  // ---------- Skapa ny exercise ----------
  const handleSubmit = async (e) => {
    e.preventDefault();
    setFormError('');
    setFormSuccess('');

    // Enkel validering
    if (!name.trim() || !muscleGroup.trim()) {
      setFormError('Namn och muskelgrupp måste fyllas i.');
      return;
    }

    try {
      setSubmitting(true);

      const url = `${API_URL}/api/exercise`;
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: name,
          muscleGroup: muscleGroup
        })
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(`Misslyckades att skapa exercise: ${response.status} – ${text}`);
      }

      setFormSuccess('Övning skapad!');
      setName('');
      setMuscleGroup('');

      // Hämta listan igen så den nya syns
      await fetchExercises();
    }
    catch (err) {
      console.error('Fel vid skapande av exercise:', err);
      setFormError(err.message || 'Ett okänt fel inträffade när övningen skulle skapas.');
    }
    finally {
      setSubmitting(false);
    }
  };

  // ---------- Rendring ----------

  return (
    <div>
      <h2>Exercises</h2>

      {/* FORMULÄR */}
      <h3>Skapa ny övning</h3>
      <form onSubmit={handleSubmit} style={{ marginBottom: '1rem' }}>
        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Namn:&nbsp;
            <input
              type="text"
              value={name}
              onChange={e => setName(e.target.value)}
            />
          </label>
        </div>

        <div style={{ marginBottom: '0.5rem' }}>
          <label>
            Muskelgrupp:&nbsp;
            <input
              type="text"
              value={muscleGroup}
              onChange={e => setMuscleGroup(e.target.value)}
              placeholder="t.ex. Ben, Bröst, Rygg"
            />
          </label>
        </div>

        <button type="submit" disabled={submitting}>
          {submitting ? 'Sparar...' : 'Spara övning'}
        </button>
      </form>

      {formError && (
        <p style={{ color: 'red' }}>{formError}</p>
      )}

      {formSuccess && (
        <p style={{ color: 'green' }}>{formSuccess}</p>
      )}

      <hr />

      {/* LISTA MED ÖVNINGAR */}
      <h3>Befintliga övningar</h3>

      {loading ? (
        <p>Loading exercises...</p>
      ) : loadError ? (
        <p style={{ color: 'red' }}>Error: {loadError}</p>
      ) : exercises.length === 0 ? (
        <p>Inga övningar hittades.</p>
      ) : (
        <ul>
          {exercises.map(ex => (
            <li key={ex.id}>
              <strong>{ex.name}</strong> – {ex.muscleGroup}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default ExercisesPage;
