import { Controls } from "./Controls"
import { DisplayTrack } from "./DisplayTrack"
import { ProgressBar } from "./ProgressBar"
import { usePlaylistContext } from "../../Contexts/PlaylistContext"
import { useEffect, useState, useRef } from "react"
import "../styles/AudioPlayer.css"
import axios from "axios"
import { Repeat } from "./Repeat"

export const AudioPlayer = () => {
    const { playlist } = usePlaylistContext();

    const [tracks, setTracks] = useState([]);
    const [trackIndex, setTrackIndex] = useState(0);
    const [currentTrack, setCurrentTrack] = useState({})
    
    const [timeProgress, setTimeProgress] = useState(0);
    const [duration, setDuration] = useState(0);

    const [isOnRepeat, setIsOnRepeat] = useState(false);

    const { setPlaylist, setIsVibeInPlaylist } = usePlaylistContext();

    const audioRef = useRef();
    const progressBarRef = useRef();

    const handleNext = async() => {
        if (isOnRepeat) {
            if (audioRef.current && currentTrack){
                audioRef.current.pause();
                audioRef.current.currentTime = 0;
                audioRef.current.play();
            }

            return;
        }

        if (trackIndex >= tracks.length - 1) {
            try{
                const response = await axios.post("/track/get-recommendations", {
                    tracksIds: tracks.map(t => t.id)
                });

                if (response.status == 200){
                    setTracks(prevTracks => [...prevTracks, ...response.data]);
                    setTrackIndex((prev) => prev + 1);
                    setCurrentTrack(response.data[0]);
                }
                console.log(response.data);
            }
            catch(e){
                console.log(e);
            }
        }
        else {
            setTrackIndex((prev) => prev + 1);
            setCurrentTrack(tracks[trackIndex + 1]);
        }
    }

    useEffect(() => {
        const getTracks = async () => {
            try{
                const response = await axios.post("/track/get-recommendations", {
                    tracksIds: []
                });
                setPlaylist(response.data); 
                setIsVibeInPlaylist(true);
            }
            catch(error){
                console.log(error);
            }
        }
        getTracks();
    }, [])

    useEffect(() => {
        setTracks(playlist);
        setTrackIndex(0);
        setCurrentTrack(playlist[0]);
        console.log(playlist);
        console.log(playlist[0]);
    }, [playlist])

    return (
        <div className="audio-player">
            <ProgressBar {... { progressBarRef, audioRef, timeProgress, duration }}/>
            <div className="controls-displaytrack-container">
                <Controls {... { audioRef, progressBarRef, duration, setTimeProgress,
                    tracks, trackIndex, setTrackIndex, setCurrentTrack, handleNext }}/>
                <DisplayTrack {... { currentTrack, audioRef, setDuration,
                    progressBarRef, handleNext }}/>
                <Repeat isOnRepeat={isOnRepeat} setIsOnRepeat={setIsOnRepeat}/>
            </div>
        </div>
    )
}