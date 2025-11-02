import { useState } from "react";
import "./styles/Album.css"
import { IoPlaySharp } from "react-icons/io5";
import { Link } from "react-router-dom";
import { usePlaylistContext } from "../Contexts/PlaylistContext";
import axios from "axios";

export const Album = ({ id, title, artist, artistId, albumCoverUrl }) => {
    const [isHovered, setIsHovered] = useState(false);
    const { setPlaylist, setIsVibePlaying, setIsVibeInPlaylist } = usePlaylistContext();

    const loadAlbumToPlaylist = async() => {
        try{
            const response = await axios.get("/track/get-tracks-by-album", {
                params: {
                    albumId: id
                }
            })

            if (response.status == 200){
                setPlaylist(response.data);
                setIsVibePlaying(false);
                setIsVibeInPlaylist(false);
            }
        }
        catch(e){
            console.log(e);
        }
    }

    return (
        <div className="album-container">
            <div
                className="album-cover-container"
                onMouseEnter={() => setIsHovered(true)}
                onMouseLeave={() => setIsHovered(false)}
                onClick={loadAlbumToPlaylist}
            >
                <img src={albumCoverUrl} alt="Album Cover" className="album-cover" />
                {isHovered && (
                    <div className="overlay">
                        <IoPlaySharp className="button-play"/>
                    </div>
                )}
            </div>
            <div className="album-info">
                <h3><Link to={`/album/${id}`} className="title-link">{title}</Link></h3>
                <p><Link to={`/artist/${artistId}`} className="artist-link">{artist}</Link></p>
            </div>
        </div>
    );
};
