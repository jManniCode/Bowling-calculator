import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import ScoreBoard from './Scoreboard'


createRoot(document.getElementById('root')).render(
  <StrictMode>
    <ScoreBoard />
  </StrictMode>,
)