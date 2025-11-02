import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './index.css'
import { BrowserRouter } from 'react-router-dom'
import "./axios/axiosDefaults.js"
import { PlaylistProvider } from './Shared/Contexts/PlaylistContext.jsx'


//Тут был <React.StrictMode> после BrowserRouter
ReactDOM.createRoot(document.getElementById('root')).render(
  <BrowserRouter>
      <PlaylistProvider>
        <App />
      </PlaylistProvider>
  </BrowserRouter>
)
