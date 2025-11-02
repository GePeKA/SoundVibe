import { useEffect, useState } from "react";
import { Album } from "../../Shared/Components/Album";
import { Track } from "../../Shared/Components/Track";
import axios from "axios";
import "./Favourite.css"

export const Favourite = () => {
    const [tracks, setTracks] = useState([]);
    const [albums, setAlbums] = useState([]);

    const getFavouriteTracks = async() => {
        try{
            const response = await axios.get("/track/get-favourite-tracks");

            if (response.status == 200){
                setTracks([...response.data]);
            }
        }
        catch(e) {
            console.log(e);
        }
    }

    const getFavouriteAlbums = async() => {
        try{
            const response = await axios.get("/album/get-favourite-albums");

            if(response.status==200){
                setAlbums([...response.data]);
            }
        }
        catch(e) {
            console.log(e);
        }
    }

    useEffect(() => {
        const fetchData = async() => {
            await getFavouriteTracks();
            await getFavouriteAlbums(); 
        };

        fetchData();
    },[])

    const albumElements = albums.map(album => <Album key={album.id} {...album}/>)
    const trackElements = tracks.map(track => <Track key={track.id} {...track}/>)

    return (
        <div className="favourites-wrapper">
            <div className="albums-wrapper">
                <h1>Альбомы</h1>
                <div className="albums-container">
                    { albumElements }
                    { albums.length == 0 && <h3>Здесь ничего нет. Добавьте альбомы в избранное, чтобы они отобразились здесь</h3>}
                </div>
            </div>
            <div className="tracks-wrapper">
                <h1>Треки</h1>
                <div className="tracks-container">
                    { trackElements }
                    { tracks.length == 0 && <h3>Здесь ничего нет. Добавьте треки в избранное, чтобы они отобразились здесь</h3> }
                </div>
            </div>
        </div>
    )
}