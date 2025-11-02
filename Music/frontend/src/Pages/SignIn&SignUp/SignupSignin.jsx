import { useEffect } from 'react';
import { CommonForm } from './CommonForm';
import { Link, useNavigate } from 'react-router-dom';
import soundVibeLogo from "../../assets/soundVibeLogo.svg"
import axios from 'axios';
import "./SignupSignin.css"

export const SignupSignin = ({ formType }) => {
    const navigate = useNavigate();

    useEffect(() => {
        const getCurrentUserDataToCheckAuthentication = async () => {
            try{
                const response = await axios.get("/user/get-personal-info");
                if (response.status == 200){
                    navigate("/"); 
                }
            }
            catch(error){
                //proceed to auth
            }
        }

        getCurrentUserDataToCheckAuthentication();
    }, [])

    return (
        <>
            <div className="page">
                <div className="logoContainer">
                    <Link to={"/"}>
                        <img src={soundVibeLogo} alt={"logo"} width={100} height={100}/>
                    </Link>
                </div>
                <div className="formContainer">
                    <div className="formHeader">
                        {formType === "signup" ? "Зарегистрироваться" : "Войти"}
                    </div>
                    <div>
                        <CommonForm formType={formType}/>
                    </div>
                </div>
            </div>
        </>
    );
};