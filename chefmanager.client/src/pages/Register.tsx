
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { RegisterForm } from "../components/RegisterForm";

function Register() {
    const [firstName, setFirstName] = useState("");
const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const navigate = useNavigate();
    const [error, setError] = useState("");

    const handleLoginClick = () => {
        navigate("/login");
    };

    const handleChange = (e) => {
        const { name, value } = e.target;
        if (name === "firstName") setFirstName(value);
if (name === "lastName") setLastName(value);
        if (name === "email") setEmail(value);
        if (name === "password") setPassword(value);
        if (name === "confirmPassword") setConfirmPassword(value);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!firstName || !lastName|| !email || !password || !confirmPassword ) {
            setError("Please fill in all fields.");
        } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
            setError("Please enter a valid email address.");
        } else if (password !== confirmPassword) {
            setError("Passwords do not match.");
        } else {
            try {
                const response = await fetch("/register", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify({
                        firstName: firstName,
lastName: lastName,
                        email: email,
                        password: password,
                    }),
                });

                if (response.ok) {
                    setError("Successful register.");
                } else {
                    setError("Error registering.");
                }
            } catch (error) {
                console.error(error);
                setError("Error registering.");
            }
        }
    };

    return (
        <div className="container mx-auto mt-10 min-h-screen">
            <h3 className="text-2xl font-semibold mb-5">Register</h3>
            <form onSubmit={handleSubmit}>
                <div className="mb-4">
                    <label htmlFor="firstName" className="block">First Name:</label>
                    <input
                        type="text"
                        id="firstName"
                        name="firstName"
                        value={firstName}
                        onChange={handleChange}
                        className="input input-bordered w-full  px-2"
                    />
                </div>
                <div className="mb-4">
                    <label htmlFor="lastName" className="block">Last Name:</label>
                    <input
                        type="text"
                        id="lastName"
                        name="lastName"
                        value={lastName}
                        onChange={handleChange}
                        className="input input-bordered w-full  px-2"
                    />
                </div>
                <div className="mb-4">
                    <label htmlFor="email" className="block">Email:</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        value={email}
                        onChange={handleChange}
                        className="input input-bordered w-full  px-2"
                    />
                </div>
                <div className="mb-4">
                    <label htmlFor="password" className="block">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        value={password}
                        onChange={handleChange}
                        className="input input-bordered w-full  px-2"
                    />
                </div>
                <div className="mb-4">
                    <label htmlFor="confirmPassword" className="block">Confirm Password:</label>
                    <input
                        type="password"
                        id="confirmPassword"
                        name="confirmPassword"
                        value={confirmPassword}
                        onChange={handleChange}
                        className="input input-bordered w-full px-2"
                    />
                </div>
                <div className="flex justify-between">
                    <button type="submit" className="btn btn-primary">Register</button>
                    <button onClick={handleLoginClick} className="btn btn-secondary">Go to Login</button>
                </div>
            </form>
            {error && <p className="text-red-500 mt-4">{error}</p>}

            <RegisterForm />
        </div>
    );
}

export default Register;



{/*import  { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";

// Define validation schema using Zod
const schema = z.object({
    username: z.string().nonempty({ message: "Username is required." }),
    email: z.string().email({ message: "Invalid email." }),
    password: z.string().min(6, { message: "Password must be at least 6 characters long." }),
    confirmPassword: z.string().min(6, { message: "Password must be at least 6 characters long." })
        .refine((confirmPassword, { password }) => confirmPassword === password, { message: "Passwords do not match." })
});

function Register() {
    const navigate = useNavigate();
    const [error, setError] = useState("");
    const { register, handleSubmit, formState: { errors } } = useForm({
        resolver: zodResolver(schema)
    });

    const handleLoginClick = () => {
        navigate("/login");
    };

    const onSubmit = async (data) => {
        try {
            const response = await fetch("/register", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({
    username: data.username,
                    email: data.email,
                    password: data.password,
                }),
            });

            if (response.ok) {
                setError("Successful register.");
            } else {
                setError("Error registering.");
            }
        } catch (error) {
            console.error("Error registering:", error);
            setError("Error registering.");
        }
    };

    return (
        <div className="container mx-auto mt-10 min-h-screen">
            <h3 className="text-2xl font-semibold mb-5">Register</h3>
            <form onSubmit={handleSubmit(onSubmit)}>
                <div className="mb-4">
                    <label htmlFor="username" className="block">Name:</label>
                    <input
                        type="text"
                        id="username"
                        name="username"
                        {...register("username")}
                        className="input input-bordered w-full  px-2"
                    />
                    {errors.username && <p className="text-red-500">{errors.username.message}</p>}
                </div>
                <div className="mb-4">
                    <label htmlFor="email" className="block">Email:</label>
                    <input
                        type="email"
                        id="email"
                        name="email"
                        {...register("email")}
                        className="input input-bordered w-full  px-2"
                    />
                    {errors.email && <p className="text-red-500">{errors.email.message}</p>}
                </div>
                <div className="mb-4">
                    <label htmlFor="password" className="block">Password:</label>
                    <input
                        type="password"
                        id="password"
                        name="password"
                        {...register("password")}
                        className="input input-bordered w-full  px-2"
                    />
                    {errors.password && <p className="text-red-500">{errors.password.message}</p>}
                </div>
                <div className="mb-4">
                    <label htmlFor="confirmPassword" className="block">Confirm Password:</label>
                    <input
                        type="password"
                        id="confirmPassword"
                        name="confirmPassword"
                        {...register("confirmPassword")}
                        className="input input-bordered w-full px-2"
                    />
                    {errors.confirmPassword && <p className="text-red-500">{errors.confirmPassword.message}</p>}
                </div>
                <div className="flex justify-between">
                    <button type="submit" className="btn btn-primary">Register</button>
                    <button onClick={handleLoginClick} className="btn btn-secondary">Go to Login</button>
                </div>
            </form>
            {error && <p className="text-red-500 mt-4">{error}</p>}
        </div>
    );
}

export default Register;*/ }