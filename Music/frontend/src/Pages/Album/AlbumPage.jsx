import { Link, useParams } from "react-router-dom"
import "./AlbumPage.css"
import { useEffect, useState } from "react";
import { Track } from "../../Shared/Components/Track";
import axios from "axios";
import { BsHeartFill } from "react-icons/bs";
import { usePlaylistContext } from "../../Shared/Contexts/PlaylistContext";

export const AlbumPage = () => {
    let { albumId } = useParams();

    const { setPlaylist, setIsVibePlaying, setIsVibeInPlaylist } = usePlaylistContext();

    const [tracks, setTracks] = useState([]);
    const [albumInfo, setAlbumInfo] = useState(null);

    const getTracksByAlbumId = async() => {
        try{
            const response = await axios.get("/track/get-track-infos-by-album", {
                params: {
                    albumId: albumId
                }
            })

            setTracks(response.data);
        }
        catch(e){
            console.log(e);
        }
    } 

    const getAlbumInfo = async() => {
        try{
            const response = await axios.get("/album/get-album-by-id", {
                params: {
                    albumId: albumId
                }
            });

            setAlbumInfo(response.data);
        }
        catch(e){
            console.log(e);
        }
    }

    useEffect(() => {
        const fetchData = async()=>{
            await getAlbumInfo();
            await getTracksByAlbumId();
        }

        fetchData();
    }, [ albumId ])

    const addAlbumToFavourites = async () => {
        try{
            const response = await axios.post("/album/add-to-favourites", { id: albumInfo.id });
            if(response.status == 200){
                setAlbumInfo(prevInfo => ({
                    ...prevInfo,
                    isFavourite: true
                }));
            }
        }
        catch(e){
            console.log(albumInfo);
            console.log(e);
        }
    }

    const removeAlbumFromFavourites = async () => {
        try{
            const response = await axios.post("/album/remove-from-favourites", { id: albumInfo.id });

            if(response.status == 200){
                setAlbumInfo(prevInfo => ({
                    ...prevInfo,
                    isFavourite: false
                }));
            }
        }
        catch(e){
            console.log(e);
        }
    }

    const loadAlbumToPlaylist = async() => {
        try{
            const response = await axios.get("/track/get-tracks-by-album", {
                params: {
                    albumId: albumInfo.id
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

    const trackElements = tracks.map(track => <Track key={track.id} {...track}/>)

    return(
        <div className="album-page-wrapper">
            {albumInfo? 
                <div className="album-page-album-info"> 
                    <img className="album-poster" src={albumInfo.albumCoverUrl} alt="album cover"/>
                    <div className="album-title-artist-wrapper"> 
                        <h1 className="album-page-album-title">{albumInfo.title}</h1>
                        <Link to={`/artist/${albumInfo.artistId}`} className="album-page-artist">{albumInfo.artist}</Link>
                        <div className="album-controls-container">
                            <button onClick={loadAlbumToPlaylist} className="listen-button">
                                Слушать
                            </button>
                            <button onClick={albumInfo.isFavourite? removeAlbumFromFavourites: addAlbumToFavourites} className="album-like-button">
                                <BsHeartFill className={`${albumInfo.isFavourite? "favourite":""}`}/>
                            </button>
                        </div>
                    </div>
                </div> : 
                <h2>Ничего не найдено</h2>}
            <div className="tracks-wrapper">
                <div className="tracks-container">
                    { trackElements }
                    { tracks.length == 0 && <h3>Авторизуйтесь для доступа к трекам</h3> }
                </div>
            </div>
        </div>
    )
}