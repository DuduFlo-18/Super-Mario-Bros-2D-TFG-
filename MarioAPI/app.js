
const express = require('express');
const mysql = require('mysql');
const app = express();
const port = process.env.PORT || 8080;

app.use(express.json());

// Conexión a MySQL usando variables de entorno
const db = mysql.createConnection({
  host: process.env.DB_HOST,
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  database: process.env.DB_NAME
});

db.connect(err => {
  if (err) {
    console.error('Error al conectar a la base de datos:', err);
    process.exit(1);
  }
  console.log('Conectado a la base de datos:', process.env.DB_NAME);
});

// Ruta para obtener el highscore
app.get('/highscore', (req, res) => {
  db.query('SELECT MAX(score) AS highscore FROM scores', (err, results) => {
    if (err) {
      console.error("Error en la consulta:", err);
      return res.status(500).send('DB error');
    }

    const max = results[0].highscore;
    res.json({ highscore: max !== null ? max : 0 });
  });
});

// Ruta para guardar un nuevo score
app.post('/highscore', (req, res) => {
  const { score } = req.body;
  if (typeof score !== 'number') {
    return res.status(400).send('Invalid score');
  }

  db.query('INSERT INTO scores (score) VALUES (?)', [score], (err, result) => {
    if (err) {
      console.error("Error al insertar:", err);
      return res.status(500).send('Insert error');
    }

    console.log("Score insertado:", score);
    res.status(201).send('Score saved');
  });
});

app.listen(port, () => {
  console.log(`Servidor corriendo en el puerto ${port}`);
});
