import { useEffect, useState } from "react";
import { Album } from "../../Shared/Components/Album";
import { Track } from "../../Shared/Components/Track";
import "./Artist.css";
import axios from "axios";
import { useParams } from "react-router-dom";
import defaultAvatar from "../../assets/default-avatar.svg"

export const Artist = () => {
    let { artistId } = useParams();

    const [tracks, setTracks] = useState([]);
    const [albums, setAlbums] = useState([]);
    const [info, setInfo] = useState(null);

    const getArtistInfo = async() => {
        try{
            const response = await axios.get("/artist/get-artist-info", {
                params: {
                    artistId: artistId
                }
            });

            setInfo(response.data);
        }
        catch(e){
            console.log(e);
        }
    }

    const getArtistTracks = async() => {
        try{
            const response = await axios.get("/track/get-artist-tracks", {
                params: {
                    artistId: artistId
                }
            });

            if (response.status == 200){
                setTracks([...response.data]);
            }
        }
        catch(e) {
            console.log(e);
        }
    }

    const getArtistAlbums = async() => {
        try{
            const response = await axios.get("/album/get-artist-albums", {
                params: {
                    artistId: artistId
                }
            })

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
            await getArtistInfo();
            await getArtistTracks();
            await getArtistAlbums(); 
        };

        fetchData();
    },[artistId])

    const albumElements = albums.map(album => <Album key={album.id} {...album}/>)
    const trackElements = tracks.map(track => <Track key={track.id} {...track}/>)


    return (
        <div className="artist-wrapper">
            { info? 
            <>
                <div className="artist-page-info">
                    <img className="artist-picture" alt="artist-pic" src={info.profilePictureUrl ? info.profilePictureUrl : defaultAvatar}/>
                    <h1 className="artist-name">{info.nickname}</h1>
                </div>
                <div className="artist-page-albums-wrapper">
                    <h1>Альбомы</h1>
                    <div className="albums-container">
                        { albumElements }
                        { albums.length == 0 && <h3>Здесь ничего нет. Исполнитель еще не добавил альбомы</h3>}
                    </div>
                </div>
                <div className="tracks-wrapper">
                    <h1>Треки</h1>
                    <div className="tracks-container">
                        { trackElements }
                        { tracks.length == 0 && <h3>Здесь ничего нет. Исполнитель еще не добавил треки</h3> }
                    </div>
                </div>
            </> :
            <h2>Нет информации</h2>}
        </div>
    )
}