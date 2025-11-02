import {BsHeartFill } from "react-icons/bs"
import unknownSong from "../../../assets/unknown-song.svg" 
import { Link } from "react-router-dom";
import axios from "axios";
import { useState } from "react";

export const DisplayTrack = ({
    currentTrack,
    audioRef,
    setDuration,
    progressBarRef,
    handleNext
}) => {

    const [isFavourite, setIsFavourite] = useState(false);

    const addTrackToFavourites = async () => {
        try{
            const response = await axios.post("/track/add-to-favourites", { id: currentTrack.id });

            if(response.status == 200){
                currentTrack.isFavourite=true;
                setIsFavourite(true);
            }
        }
        catch(e){
            console.log(e);
        }
    }

    const removeTrackFromFavourites = async () => {
        try{
            const response = await axios.post("/track/remove-from-favourites", { id: currentTrack.id });

            if(response.status == 200){
                currentTrack.isFavourite=false;
                setIsFavourite(false);
            }
        }
        catch(e){
            console.log(e);
        }
    }
    
    const handleLoadedMetadata = () => {
        const seconds = audioRef.current.duration;
        setDuration(seconds);
        progressBarRef.current.max = seconds;
        setIsFavourite(currentTrack.isFavourite);
        console.log(currentTrack);
    }

    return (
        <div >
            <audio 
                src={currentTrack? currentTrack.url : ""}
                ref={audioRef}
                onLoadedMetadata={handleLoadedMetadata}
                onEnded={handleNext}
            />
            <div className="audio-info">
                <img className="song-image" src={currentTrack && currentTrack.trackCoverUrl ? 
                    currentTrack.trackCoverUrl : unknownSong } alt="songLogo"/>
                <div className="track-info-player">
                    <Link className="song-title">{currentTrack? currentTrack.title : ""}</Link>
                    <Link to={`artist/${currentTrack? currentTrack.artistId : ""}`} className="song-artist">{currentTrack? currentTrack.artist : ""}</Link>
                </div>
                { currentTrack ? (
                    <button onClick={isFavourite? removeTrackFromFavourites : addTrackToFavourites} className="like-button-player">
                        <BsHeartFill className={`${isFavourite? "favourite":""}`}/>
                    </button>): null
                }
            </div>
        </div>
    )
}