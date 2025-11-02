import { useState } from "react";
import "./styles/Track.css"
import { Link } from "react-router-dom";
import unknownSongLogo from "../../assets/unknown-song.svg"
import { IoPlayCircle } from "react-icons/io5";
import axios from "axios";
import { usePlaylistContext } from "../Contexts/PlaylistContext";
import { BsHeartFill } from "react-icons/bs";

export const Track = ({id, title, isFavourite, artist, artistId, trackCoverUrl}) => {
    const [isHovered, setIsHovered] = useState(false);
    const [isTrackFavourite, setIsTrackFavourite] = useState(isFavourite);

    const { setPlaylist, setIsVibePlaying, setIsVibeInPlaylist } = usePlaylistContext();

    const setTrackInPlaylist = async() => {
        try{
            const response = await axios.get("/track/get-track-by-id", {
                params: {
                    trackId: id
                }
            })

            if (response.status == 200){
                setPlaylist([response.data]);
                setIsVibePlaying(false);
                setIsVibeInPlaylist(false);
            }
        }
        catch(e){
            console.log(e);
        }
    }

    const addTrackToFavourites = async () => {
        try{
            const response = await axios.post("/track/add-to-favourites", { id: id });

            if(response.status == 200){
                setIsTrackFavourite(true);
            }
        }
        catch(e){
            console.log(e);
        }
    }

    const removeTrackFromFavourites = async () => {
        try{
            const response = await axios.post("/track/remove-from-favourites", { id: id });

            if(response.status == 200){
                setIsTrackFavourite(false);
            }
        }
        catch(e){
            console.log(e);
        }
    }

    return(
        <div 
            className="track-container"
            onMouseEnter={() => setIsHovered(true)}
            onMouseLeave={() => setIsHovered(false)}
        >
            <div className="track-cover-container"
                onClick={setTrackInPlaylist}>
                {isHovered && (
                    <div className="play-icon">
                        <IoPlayCircle className="track-play-svg"/>
                    </div>
                )}
                <img src={trackCoverUrl? trackCoverUrl : unknownSongLogo} alt="Track Cover" className={`track-cover ${isHovered? "hovered": ""}`}/>
            </div>
            
            <div className="track-info">
                <p className="track-title">{title}</p>
                <Link to={`/artist/${artistId}`} className="track-author">{artist}</Link>
            </div>
            <div className="like-container">
                <button onClick={isTrackFavourite? removeTrackFromFavourites: addTrackToFavourites} className="like-button">
                    <BsHeartFill className={`${isTrackFavourite? "favourite":""}`}/>
                </button>
            </div>
            
        </div>
    )
}