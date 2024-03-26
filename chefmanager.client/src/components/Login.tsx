import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import UserDTO from "../DTOs/UserDTO";
import useAuthStore from "../store/useAuthStore";

// Define validation schema using Zod
const schema = z.object({
    email: z.string().email({
        message: "Invalid email.",
    }),
    password: z.string().min(6, {
        message: "Incorrect password",
    }),
    rememberme: z.boolean(),
});

function Login() {
    const navigate = useNavigate();
    const setUserAsync = useAuthStore((state) => state.setUserAsync);
    const [error, setError] = useState("");
    const { register, handleSubmit, formState: { errors } } = useForm({
        resolver: zodResolver(schema),
        defaultValues: {
email: "",
            password: "",
            rememberme: false,
        },
    });

    const handleRegisterClick = () => {
        navigate("/register");
    };
    const handleCloseModal = () => {
        document.getElementById('my_modal_2').close();
    };

    const fetchUserData = async () => {
        try {
            const response = await fetch("/pingauth", {
                method: "GET",
            });
            if (response.ok) {
                const userData = await response.json();
                return userData;
            } else {
                throw new Error("Failed to fetch user data");
            }
        } catch (error) {
            console.error("Error fetching user data:", error);
            throw error;
        }
    };

    const onSubmit = async (data) => {
        try {
            const loginUrl = data.rememberme ? "/login?useCookies=true" : "/login?useSessionCookies=true";
            const response = await fetch(loginUrl, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
                    email: data.email,
                    password: data.password,
                }),
            });
            if (response.ok) {
                setError("Successful Login.");
                const userData = await fetchUserData();
                setUserAsync(userData);
                handleCloseModal();
                navigate("/");
            } else {
                setError("Error Logging In.");
            }
        } catch (error) {
            console.error("Error logging in:", error);
            setError("Error Logging In.");
        }
    };

    return (
        <div className="container mx-auto">
            <h3 className="text-2xl font-bold mb-4">Login</h3>
            <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
                <div className="flex flex-col">
                    <label htmlFor="email" className="text-sm">Email:</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        {...register("email")}
                        className="form-input mt-1  px-2"
                    />
                    {errors.email && <p className="text-red-500">{errors.email.message}</p>}
                </div>
                <div className="flex flex-col">
                    <label htmlFor="password" className="text-sm">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        {...register("password")}
                        className="form-input mt-1 px-2"
                    />
                    {errors.password && <p className="text-red-500">{errors.password.message}</p>}
                </div>
                <div className="flex items-center">
                    <input
                        type="checkbox"
                        id="rememberme"
                        name="rememberme"
                        {...register("rememberme")}
                        className="form-checkbox h-4 w-4 text-indigo-600"
                    />
                    <label htmlFor="rememberme" className="ml-2 text-sm">Remember Me</label>
                </div>
                <div className=" flex justify-between">
                    <button type="submit" className="btn btn-primary" >Login</button>
                    <button onClick={() => { handleRegisterClick(), handleCloseModal() }} type="button" className="btn btn-secondary" >Register</button>
                </div>
            </form>
            {/*          {error && <p className="text-red-500">{error}</p>}*/ }
        </div>
    );
}

export default Login;
