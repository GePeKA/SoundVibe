import { useState } from "react";
import { useNavigate } from "react-router-dom"
import searchIcon from "../../../assets/Search.svg"
import "../styles/SearchPanel.css"

export const SearchPanel = () => {
    const navigate = useNavigate();
    const [searchText, setSearchText] = useState("");

    const navigateToSearch = () => {
        navigate(`/search?text=${searchText}`)
    }

    const handleKeyDown = (e) => {
        if (e.key == 'Enter'){
            navigateToSearch();
        }
    }

    return (
        <>
            <div className="search-panel">
                <input className="search-input" onKeyDown={handleKeyDown} type="text" placeholder="Поиск по названию" onChange={(e) => setSearchText(e.target.value)} />
                <button type="button" className="search-icon-container" onClick={navigateToSearch}>
                    <img className="search-icon" src={searchIcon} alt="search"/>
                </button>   
            </div>
        </>
    )
}