import { useEffect, useState } from "react"
import { NavigatePanel } from "./components/NavigatePanel"
import { SearchPanel } from "./components/SearchPanel"
import "./styles/Header.css"
import { useNavigate } from "react-router-dom"
import { ProfilePanel } from "./components/ProfilePanel"
import axios from "axios"
import { usePlaylistContext } from "../Contexts/PlaylistContext"

export const Header = () => {
    const [user, setUser] = useState(null); 
    const { setPlaylist } = usePlaylistContext();
    const navigate = useNavigate();

    const navigateToSignIn = () => {
        navigate("/signin");
    }
    
    const logout = () => {
        sessionStorage.removeItem("accessToken");
        setPlaylist([]);
        setUser(null);
    }

    useEffect(() => {
        const getUser = async () => {
            try{
                const response = await axios.get("/user/get-personal-info");
                if(response.status == 200){
                    setUser(response.data);
                }
            }
            catch(error){
                //nothing
            }
        }

        getUser();
    }, []);

    return (
        <>
            <header>
                <NavigatePanel/>
                <SearchPanel/>
                <div className="profile-container">
                    { user == null && <button onClick={navigateToSignIn} className="signin-button">Войти</button>}
                    { user != null && <ProfilePanel user={user} logout={logout}/>}
                </div>
            </header>
        </>
    )
}