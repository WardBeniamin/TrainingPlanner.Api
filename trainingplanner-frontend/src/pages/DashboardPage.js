import { useEffect, useState } from 'react';

const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:7296';

function DashboardPage() {
  const [userCount, setUserCount] = useState(0);
  const [workoutCount, setWorkoutCount] = useState(0);
  const [exerciseCount, setExerciseCount] = useState(0);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  useEffect(() => {
    const loadStats = async () => {
      try {
        setLoading(true);
        setError('');

        // Hämta alla tre samtidigt
        const [usersRes, workoutsRes, exercisesRes] = await Promise.all([
          fetch(`${API_URL}/api/user`),
          fetch(`${API_URL}/api/workout`),
          fetch(`${API_URL}/api/exercise`)
        ]);

        if (!usersRes.ok || !workoutsRes.ok || !exercisesRes.ok) {
          throw new Error('Kunde inte läsa statistik från API.');
        }

        const users = await usersRes.json();
        const workouts = await workoutsRes.json();
        const exercises = await exercisesRes.json();

        setUserCount(users.length);
        setWorkoutCount(workouts.length);
        setExerciseCount(exercises.length);
      } catch (err) {
        console.error('Fel på dashboard:', err);
        setError(err.message || 'Ett okänt fel inträffade.');
      } finally {
        setLoading(false);
      }
    };

    loadStats();
  }, []);

  if (loading) {
    return <p>Loading dashboard...</p>;
  }

  if (error) {
    return <p style={{ color: 'red' }}>Error: {error}</p>;
  }

  return (
    <div>
      <h2>Dashboard</h2>
      <p>Snabb översikt över ditt träningssystem.</p>

      <ul>
        <li><strong>Användare:</strong> {userCount}</li>
        <li><strong>Workouts:</strong> {workoutCount}</li>
        <li><strong>Övningar:</strong> {exerciseCount}</li>
      </ul>

      <p>
        Gå till <strong>Users</strong>, <strong>Workouts</strong> eller <strong>Exercises</strong> i menyn
        för att lägga till mer data.
      </p>
    </div>
  );
}

export default DashboardPage;
