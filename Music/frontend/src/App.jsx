import { Route, Routes, useLocation } from "react-router-dom"
import { Header } from "./Shared/Header/Header";
import { Main } from "./Pages/Main/Main";
import { SignupSignin } from "./Pages/SignIn&SignUp/SignupSignin";
import { Footer } from "./Shared/Footer/Footer";
import { Search } from "./Pages/Search/Search"
import { Music } from "./Pages/Music/Music";
import { Artist } from "./Pages/Artist/Artist";
import { Favourite } from "./Pages/Favourite/Favourite";
import { AlbumPage } from "./Pages/Album/AlbumPage";

function App() {
    const location = useLocation();
    const pathsWithNoHeaderAndFooter = ["/signin", "/signup"]

    return (
        <>
            {!pathsWithNoHeaderAndFooter.includes(location.pathname) && <Header/> }
            <Routes>
                <Route path="/" element={<Main/>}/>
                <Route path="/signup" element={<SignupSignin formType="signup"/>}/>
                <Route path="/signin" element={<SignupSignin formType="signin"/>}/>
                <Route path="/search" element={<Search/>}/>
                <Route path="/music" element={<Music/>}/>
                <Route path="/artist/:artistId" element={<Artist/>}/>
                <Route path="/album/:albumId" element={<AlbumPage/>}/>
                <Route path="/favourite" element={<Favourite/>}/> 
            </Routes>
            {!pathsWithNoHeaderAndFooter.includes(location.pathname) && <Footer/> }
        </>
    )
}

export default App
