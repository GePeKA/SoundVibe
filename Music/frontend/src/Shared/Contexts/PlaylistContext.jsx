import { useState } from "react";
import { createContext } from "react";
import { useContext } from "react";

const PlaylistContext = createContext();

export const PlaylistProvider = ({ children }) => {
    const [playlist, setPlaylist] = useState([]);
    const [isVibePlaying, setIsVibePlaying] = useState(false);
    const [isVibeInPlaylist, setIsVibeInPlaylist] = useState(false);
  
    return (
        <PlaylistContext.Provider value={{ playlist, setPlaylist, isVibePlaying, setIsVibePlaying, isVibeInPlaylist, setIsVibeInPlaylist }}>
            {children}
        </PlaylistContext.Provider>
    );
};

export const usePlaylistContext = () => {
    return useContext(PlaylistContext);
}