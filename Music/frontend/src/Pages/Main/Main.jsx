import { useEffect, useRef } from "react";
import { usePlaylistContext } from "../../Shared/Contexts/PlaylistContext"
import axios from "axios";
import visualiser from "../../assets/viber.mp4"
import "./styles/Main.css"
import { IoPauseSharp, IoPlaySharp } from "react-icons/io5";

export const Main = () => {
    const { playlist, setPlaylist, isVibePlaying, setIsVibePlaying, isVibeInPlaylist, setIsVibeInPlaylist } = usePlaylistContext();
    const videoRef = useRef();

    const toggleVibe = async () => {
        if (!isVibeInPlaylist) {
            try{
                let listenedTracksIds = [];
                if (isVibeInPlaylist){
                    listenedTracksIds = playlist.map(t => t.id);
                }
                const response = await axios.post("/track/get-recommendations", {
                    tracksIds: listenedTracksIds
                });
                if (response.status==200){
                    setPlaylist(response.data);
                    setIsVibeInPlaylist(true);
                    setIsVibePlaying(prev => !prev); 
                }
                
            }
            catch(error){
                console.log(error);
            }
        }
        else{
            setIsVibePlaying(prev => !prev);
        }
    }

    useEffect(() => {
        videoRef.current.play();
    },[])

    return (
        <div className="mainpage-wrapper">
            <div className="vibe-wrapper">
                <div className="background-overlay"></div>
                <video className="background-video" src={visualiser} ref={videoRef} muted loop playsInline/>
                
                <div className="vibe-content">
                    <h1>Моя волна</h1>
                    <button onClick={toggleVibe} className="play-button">
                        { isVibePlaying? <IoPauseSharp className="play-svg"/> : <IoPlaySharp className="play-svg"/> }
                    </button>
                </div>
            </div>
        </div>
    )
}