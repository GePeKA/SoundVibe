import { Link, useLocation, useNavigate } from "react-router-dom"
import soundVibeLogo from "../../../assets/soundVibeLogo.svg"
import "../styles/NavigatePanel.css"

export const NavigatePanel = () => {
    const location = useLocation();
    const navigate = useNavigate();

    const navigateToMain = () => {
        navigate("/");
    }

    return (
        <>
            <img className="logo" alt="logo" src={soundVibeLogo} onClick={navigateToMain}></img>
            <div className={location.pathname == "/" ? "navigate-element active" : "navigate-element"}>
                <Link className="link" to="/">Главная</Link>
            </div>
            <div className={location.pathname == "/favourite" ? "navigate-element active" : "navigate-element"}>
                <Link className="link" to="/favourite">Избранное</Link>
            </div>
        </>
    )
}