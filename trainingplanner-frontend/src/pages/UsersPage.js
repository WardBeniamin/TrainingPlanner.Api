import { useEffect, useState } from 'react';

// Läser adressen till API:t från .env (backup om env saknas)
const API_URL = process.env.REACT_APP_API_URL || 'https://localhost:7296';

function UsersPage() {
  // ----- State för listan -----
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [loadError, setLoadError] = useState('');

  // ----- State för formuläret -----
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [formError, setFormError] = useState('');
  const [formSuccess, setFormSuccess] = useState('');
  const [submitting, setSubmitting] = useState(false);

  // ------ Funktion för att hämta users från API ------
  const fetchUsers = async () => {
    try {
      setLoading(true);
      setLoadError('');

      const url = `${API_URL}/api/user`;
      console.log('Hämtar users från:', url);

      const response = await fetch(url);

      if (!response.ok) {
        const text = await response.text();
        throw new Error(`API-svar: ${response.status} – ${text}`);
      }

      const contentType = response.headers.get('content-type') || '';
      if (!contentType.includes('application/json')) {
        const text = await response.text();
        throw new Error('API returnerade inte JSON: ' + text.slice(0, 100));
      }

      const data = await response.json();
      setUsers(data);
    }
    catch (err) {
      console.error('Fel när users hämtades:', err);
      setLoadError(err.message || 'Ett okänt fel inträffade.');
    }
    finally {
      setLoading(false);
    }
  };

  // Kör när sidan laddas första gången
  useEffect(() => {
    fetchUsers();
  }, []);

  // ------ Hantera submit för formuläret ------
  const handleSubmit = async (e) => {
    e.preventDefault(); // stoppa vanlig form-submit

    setFormError('');
    setFormSuccess('');

    // Enkel validering
    if (!name.trim() || !email.trim()) {
      setFormError('Namn och e-post får inte vara tomma.');
      return;
    }

    if (!email.includes('@')) {
      setFormError('E-postadressen verkar inte vara giltig.');
      return;
    }

    try {
      setSubmitting(true);

      const url = `${API_URL}/api/user`;
      const response = await fetch(url, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          name: name,
          email: email
        })
      });

      if (!response.ok) {
        const text = await response.text();
        throw new Error(`Misslyckades att skapa user: ${response.status} – ${text}`);
      }

      setFormSuccess('Användare skapad!');
      setName('');
      setEmail('');

      // Hämta listan igen så den nya visas
      await fetchUsers();
    }
    catch (err) {
      console.error('Fel vid skapande av user:', err);
      setFormError(err.message || 'Ett okänt fel inträffade när användaren skulle skapas.');
    }
    finally {
      setSubmitting(false);
    }
  };

  // ---------- Render ----------

  return (
    <div>
      <h2>Users</h2>

      {/* FORMULÄR FÖR NY USER */}
      <h3>Skapa ny användare</h3>
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
            E-post:&nbsp;
            <input
              type="email"
              value={email}
              onChange={e => setEmail(e.target.value)}
            />
          </label>
        </div>

        <button type="submit" disabled={submitting}>
          {submitting ? 'Sparar...' : 'Spara användare'}
        </button>
      </form>

      {formError && (
        <p style={{ color: 'red' }}>{formError}</p>
      )}

      {formSuccess && (
        <p style={{ color: 'green' }}>{formSuccess}</p>
      )}

      <hr />

      {/* LISTA MED USERS */}
      <h3>Befintliga användare</h3>

      {loading ? (
        <p>Loading users...</p>
      ) : loadError ? (
        <p style={{ color: 'red' }}>Error: {loadError}</p>
      ) : users.length === 0 ? (
        <p>Inga användare hittades.</p>
      ) : (
        <ul>
          {users.map(user => (
            <li key={user.id}>
              <strong>{user.name}</strong> – {user.email}
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

export default UsersPage;
