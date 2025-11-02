import { useCallback, useRef, useEffect, useState } from "react"

import {
    IoPlaySkipBackSharp,
    IoPlaySkipForwardSharp,
    IoPlaySharp,
    IoPauseSharp,
} from 'react-icons/io5';
  
import {
    IoMdVolumeHigh,
    IoMdVolumeOff,
    IoMdVolumeLow,
} from 'react-icons/io';
import { usePlaylistContext } from "../../Contexts/PlaylistContext";

export const Controls = ({
        audioRef,
        progressBarRef,
        duration,
        setTimeProgress,
        tracks,
        trackIndex,
        setTrackIndex,
        setCurrentTrack,
        handleNext,
}) => {

    const [isPlaying, setIsPlaying] = useState(false);
    const [volume, setVolume] = useState(60);
    const [muteVolume, setMuteVolume] = useState(false);

    const { isVibePlaying, setIsVibePlaying, isVibeInPlaylist } = usePlaylistContext();

    const togglePlayPause = () => {
        setIsPlaying((prev) => !prev);
        if(isVibeInPlaylist) {
            setIsVibePlaying(prev => !prev);
        }
    };

    useEffect(() => {
        setIsPlaying(isVibePlaying);
    }, [isVibePlaying]);

    const playAnimationRef = useRef();

    const repeat = useCallback(() => {
        const currentTime = audioRef.current.currentTime;
        setTimeProgress(currentTime);
        progressBarRef.current.value = currentTime;
        progressBarRef.current.style.setProperty(
            '--range-progress',
            `${(progressBarRef.current.value / duration) * 100}%`
        );

        playAnimationRef.current = requestAnimationFrame(repeat);
    }, [audioRef, duration, progressBarRef, setTimeProgress]);

    useEffect(() => {
        if (isPlaying){
            audioRef.current.play();
        } else{
            audioRef.current.pause();
        }

        playAnimationRef.current = requestAnimationFrame(repeat);
    }, [isPlaying, audioRef, repeat])

    const handlePrevious = () => {
        if (trackIndex === 0) {
            //nothing
        } else {
            setTrackIndex((prev) => prev - 1);
            setCurrentTrack(tracks[trackIndex - 1]);
        }
    };

    useEffect(() => {
        if (audioRef) {
            audioRef.current.volume = volume / 100;
            audioRef.current.muted = muteVolume;
        }
    }, [volume, audioRef, muteVolume]);

    return (
        <div className="controls-wrapper">
            <div className="controls">
                <button className="controls-button" onClick={handlePrevious}>
                    <IoPlaySkipBackSharp />
                </button>
        
                <button className="controls-button" onClick={togglePlayPause}>
                    {isPlaying ? <IoPauseSharp /> : <IoPlaySharp />}
                </button>
                <button className="controls-button" onClick={handleNext}>
                    <IoPlaySkipForwardSharp />
                </button>
            </div>
            <div className="volume">
                <button className="controls-button" onClick={() => setMuteVolume((prev) => !prev)}>
                    { muteVolume || volume < 5 ? (
                        <IoMdVolumeOff />
                    ) : volume < 40 ? (
                        <IoMdVolumeLow />
                    ) : (
                        <IoMdVolumeHigh />
                    )}
                </button>
                <input
                    type="range"
                    min={0}
                    max={100}
                    value={volume}
                    onChange={(e) => setVolume(e.target.value)}
                    style={{
                        background: `linear-gradient(to right, #f50 ${volume}%, #ccc ${volume}%)`,
                    }}
                />
            </div>
        </div>
    );
}