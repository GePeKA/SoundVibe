import { useState } from "react"
import "./Search.css"
import { useSearchParams } from "react-router-dom";
import { useEffect } from "react";
import axios from "axios";
import { Album } from "../../Shared/Components/Album";
import { Track } from "../../Shared/Components/Track";

export const Search = () => {
    const [searchParams] = useSearchParams();

    const [tracks, setTracks] = useState([]);
    const [albums, setAlbums] = useState([]);

    const [allTracksLoaded, setAllTracksLoaded] = useState(false);
    const [allAlbumsLoaded, setAllAlbumsLoaded] = useState(false);

    const [trackRange, setTrackRange] = useState({ from: 0, to: 10 });
    const [albumRange, setAlbumRange] = useState({ from: 0, to: 8 });

    const getTracksBySearch = async(from, to) => {
        try{
            const searchParam = searchParams.get("text");
            const response = await axios.get("/track/get-tracks-by-search", {
                params: {
                    text: searchParam,
                    from: from,
                    to: to
                }
            })

            if (response.status == 200){
                setTracks(prevTracks => [...prevTracks, ...response.data]);
                console.log(trackRange.from);
                console.log(trackRange.to);
                if(response.data.length != to-from) {
                    setAllTracksLoaded(true);
                }
                else {
                    setTrackRange(prevRange => ({ from: prevRange.to, to: prevRange.to + (prevRange.to-prevRange.from) }));
                }
                console.log(response.data);
            }
        }
        catch(e) {
            console.log(e);
        }
    }

    const getAlbumsBySearch = async(from, to) => {
        try{
            const searchParam = searchParams.get("text");
            const response = await axios.get("/album/get-albums-by-search", {
                params: {
                    text: searchParam,
                    from: from,
                    to: to
                }
            })

            if(response.status==200){
                setAlbums(prevAlbums => [...prevAlbums, ...response.data]);
                
                if(response.data.length != to - from){
                    setAllAlbumsLoaded(true);
                }
                else{
                    setAlbumRange(prevRange => ({ from: prevRange.to, to: prevRange.to + (prevRange.to-prevRange.from) }));
                }
            }
        }
        catch(e) {
            console.log(e);
        }
    }

    useEffect(() => {
        setAllAlbumsLoaded(false);
        setAllTracksLoaded(false);
        
        setTrackRange({from: 0, to: 10});
        setAlbumRange({from: 0, to: 8});

        const fetchData = async() => {
            setAlbums([]);
            setTracks([]);
            
            await getTracksBySearch(0, 10);
            await getAlbumsBySearch(0, 8); 
        };

        fetchData();
    }, [searchParams])

    const albumElements = albums.map(album => <Album key={album.id} {...album}/>)
    const trackElements = tracks.map(track => <Track key={track.id} {...track}/>)

    return (
        <div className="search-wrapper">
            <div className="albums-wrapper">
                <h1>Альбомы</h1>
                <div className="albums-container">
                    { albumElements }
                    { albums.length == 0 && <h3>Ничего не найдено</h3>}
                </div>
                { albums.length == 0 || allAlbumsLoaded? 
                    null:
                    <div className="button-container">
                        <button onClick={() => getAlbumsBySearch(albumRange.from, albumRange.to)} className="load-button" type="button">Загрузить еще</button>
                    </div>
                }
            </div>
            <div className="tracks-wrapper">
                <h1>Треки</h1>
                <div className="tracks-container">
                    { trackElements }
                    { tracks.length == 0 && <h3>Ничего не найдено</h3> }
                </div>
                { tracks.length == 0 || allTracksLoaded? 
                    null:
                    <div className="button-container">
                        <button onClick={() => getTracksBySearch(trackRange.from, trackRange.to)} className="load-button" type="button">Загрузить еще</button>
                    </div>
                }
            </div>
        </div>
    )
}