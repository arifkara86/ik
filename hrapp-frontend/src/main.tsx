// src/main.tsx
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App.tsx'; // App component'ini import et
import './index.css'; // Genel stiller

// Root elementi seç ve React uygulamasını render et
const rootElement = document.getElementById('root');
if (rootElement) {
  ReactDOM.createRoot(rootElement).render(
    <React.StrictMode>
      <App />
    </React.StrictMode>,
  );
} else {
  console.error("Failed to find the root element. Ensure there is an element with ID 'root' in your index.html.");
}