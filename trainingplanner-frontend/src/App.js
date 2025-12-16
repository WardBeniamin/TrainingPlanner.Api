import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import DashboardPage from './pages/DashboardPage';
import UsersPage from './pages/UsersPage';
import WorkoutsPage from './pages/WorkoutsPage';
import ExercisesPage from './pages/ExercisesPage';

function App() {
  return (
    <Router>
      <div style={{ padding: '1rem' }}>
        <h1>Training Planner</h1>

        {/* Enkel meny */}
        <nav style={{ marginBottom: '1rem' }}>
          <Link to="/" style={{ marginRight: '1rem' }}>Dashboard</Link>
          <Link to="/users" style={{ marginRight: '1rem' }}>Users</Link>
          <Link to="/workouts" style={{ marginRight: '1rem' }}>Workouts</Link>
          <Link to="/exercises">Exercises</Link>
        </nav>

        {/* Här byts innehållet beroende på URL */}
        <Routes>
          <Route path="/" element={<DashboardPage />} />
          <Route path="/users" element={<UsersPage />} />
          <Route path="/workouts" element={<WorkoutsPage />} />
          <Route path="/exercises" element={<ExercisesPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
