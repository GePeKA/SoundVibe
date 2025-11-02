import { Avatar, Button, Divider, Menu, MenuItem, Typography } from "@mui/material";
import axios from "axios";
import { jwtDecode } from "jwt-decode";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

export const ProfilePanel= ({user, logout}) => {
    const navigate = useNavigate();
    const token = sessionStorage.getItem("accessToken");
    const decodedUser = jwtDecode(token);

    const [anchorEl, setAnchorEl] = useState(null);
    const open = Boolean(anchorEl);

    const handleClick = (event) => {
        setAnchorEl(event.currentTarget);
    };

    const handleClose = () => {
        setAnchorEl(null);
    };

    const handleItemClick = (path) => {
        navigate(path);
        handleClose();
    }

    const handleLogout = () => {
        logout();
    }

    const becomeArtist = async () => {
        try{
            const response = await axios.post("user/become-artist");

            if (response.status == 201){
                logout();
            }
        }
        catch(e){
            console.log(e);
        }
    }

    return (
        <div>
            <Button id="basic-button" onClick={handleClick} sx={{ padding: "2px" }}>
                <Avatar src={user.profilePictureUrl} sx={{
                    height: "3rem",
                    width: "3rem"
                }}/>
            </Button>
            <Menu
                id="basic-menu"
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                slotProps={{
                    paper: {
                        style: {
                            backgroundColor: "#454545"
                        }
                    }
                }}
            >
                <Typography sx={{ padding: "0.2rem", color: "white", fontWeight:"600", marginLeft: "0.7rem"}}>{user.nickname}</Typography>
                <Divider sx={{ borderBottomWidth: "3px" }}/>
                <MenuItem sx={{ color: "white"}} onClick={() => handleItemClick("/personalInfo")}>Личная информация</MenuItem>
                { decodedUser.role == "artist" && <MenuItem sx={{ color: "white"}} onClick={() => handleItemClick(`/artist/${decodedUser.id}`)}>Страница музыканта</MenuItem> }
                { decodedUser.role == "artist" && <MenuItem sx={{ color: "white"}} onClick={() => handleItemClick(`/music`)}>Управление музыкой</MenuItem> }
                { decodedUser.role == "user" && <MenuItem sx={{color: "white"}} onClick={becomeArtist}>Стать исполнителем</MenuItem> }
                <MenuItem sx={{
                    color: "white",
                    ":hover": {
                        backgroundColor: "#e50000",
                    }
                }} onClick={handleLogout}>Выход</MenuItem>
            </Menu>
        </div>
    );
}