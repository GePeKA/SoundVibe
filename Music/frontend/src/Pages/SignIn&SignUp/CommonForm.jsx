import { useState } from 'react';
import { Formik, Form, Field, ErrorMessage } from 'formik';
import { Alert } from '@mui/material';
import { Link, useNavigate } from 'react-router-dom';
import axios from 'axios';

export const CommonForm = ({formType}) => {
    const [response, setResponse] = useState(null);
    const navigate = useNavigate();

    const validateSignin = (values) => {
        const errors = {};

        if (!values.email) {
            errors.email = 'Обязательное поле';
        }

        if (!values.password) {
            errors.password = "Обязательное поле";
        } else if(values.password.length < 8) {
            errors.password = "Минимальная длина пароля - 8 символов";
        } else if (values.password.length > 30) {
            errors.password = "Максимальная длина пароля - 30 символов"
        } else if (!/^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@;.,$!%*?&]).+$/.test(values.password)) {
            errors.password = "Пароль должен содержать хотя бы одну букву, цифру и спецсимвол";
        }

        return errors;
    }

    const validateSignup = (values) => {
        const errors = {};
        
        if (!values.nickname){
            errors.nickname = 'Обязательное поле';
        } else if (values.nickname.length < 4){
            errors.nickname = 'Минимальная длина логина - 4 символов';
        } else if (values.nickname.length > 25){
            errors.nickname = 'Максимальная длина логина - 25 символов'
        } else if (!/^[a-zA-Z0-9_]+$/.test(values.nickname)){
            errors.nickname = "Запрещенные символы";
        }

        if (!values.email) {
            errors.email = 'Обязательное поле';
        } else if (!/^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$/i.test(values.email)) {
            errors.email = 'Неправильный адрес почты';
        }

        if (!values.password) {
            errors.password = "Обязательное поле";
        } else if(values.password.length < 8) {
            errors.password = "Минимальная длина пароля - 8 символов";
        } else if (values.password.length > 30) {
            errors.password = "Максимальная длина пароля - 30 символов"
        } else if (!/^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@;.,$!%*?&]).+$/.test(values.password)) {
            errors.password = "Пароль должен содержать хотя бы одну букву, цифру и спецсимвол";
        }
      
        return errors;
    };
    
    const initialSignupValues = {
        nickname: '',
        email: '',
        password: '',
    };
    
    const initialSigninValues = {
        email: '',
        password: '',
        rememberMe: false,
    }

    const handleSubmit = async (values) => {    
        try {
            const resp = await axios.post(
                `/auth/${formType}`,
                values
            );

            if (resp.status == 200 || resp.status == 201){
                let message = formType === "signin" ? "Успешный вход" : "Успешная регистрация";
                setResponse({Success: true, Message: message});

                if (formType == "signin") {
                    let accessToken = resp.data;
                    sessionStorage.setItem("accessToken", accessToken);
                }

                await new Promise((resolve => setTimeout(resolve, 1000)));

                navigate(formType == "signin" ? "/" : "/signin");
            }
        }
        catch (error) {
            if (error.response && error.response.status == 400) {
                let errorText = error.response.data[0];
                setResponse({Success: false, Message: errorText})
            }
            else{
                setResponse({Success: false, Message: `Ошибка: ${error}`})
            }
        }
    };

    return (
        <Formik 
            initialValues={formType === "signup" ? initialSignupValues : initialSigninValues} 
            onSubmit={handleSubmit} 
            validate={formType === "signup"? validateSignup: validateSignin}
        >
            <Form id="form">
                { formType === 'signup' ? (
                    <div className="inputWrapper">
                        <Field type="text" name="nickname" placeholder="Логин"/>
                        <ErrorMessage name="nickname" component="span"/>
                    </div>
                ) : null}
                <div className="inputWrapper">
                    <Field type="email" name="email" placeholder="Почта" />
                    <ErrorMessage name="email" component="span"/>
                </div>

                <div className="inputWrapper">
                    <Field type="password" name="password" placeholder="Пароль"/>
                    <ErrorMessage name="password" component="span"/>
                </div>
                
                <div className="inputWrapper">
                    <button type="submit" className="submitButton">{formType === 'signin' ? 'Войти' : 'Зарегистрироваться'}</button>
                </div>

                {formType === 'signin' ? (
                    <div className="rememberCheckbox">
                        <Field type="checkbox" name="rememberMe" />
                        <label>Запомнить меня</label>
                    </div>
                ) : null}

                <div className="redirect">
                    <p>{formType === "signin" ? "Новенький в SoundVibe?" : "Уже есть аккаунт?"}</p>
                    <Link to={`/${formType === "signin" ? "signup" : "signin"}`}>{formType ==="signin" ? "Зарегистрироваться" : "Войти"}</Link>
                </div>

                <Alert severity={response != null && response.Success ? "success" : "error"} 
                    variant="filled"
                    onClose = {() => setResponse(null)}
                    icon={false}
                    sx={{
                        marginTop: "3%", 
                        display: response != null ? "flex" : "none",
                        paddingTop: "0%",
                        paddingBottom: "0%"
                    }}>
                    {response != null ? response.Message : ""}
                </Alert>
            </Form>
        </Formik>
    );
};