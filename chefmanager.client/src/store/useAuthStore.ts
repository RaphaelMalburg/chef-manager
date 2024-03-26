import { create } from 'zustand'
import { persist } from 'zustand/middleware'
import  UserDTO  from '../DTOs/UserDTO'

type AuthStore = {
authorized: boolean
    user: UserDTO
    setAuthorized: (authorized: boolean) => void
    setUserAsync: (user: UserDTO) => Promise<void>
}

const useAuthStore = create(
    persist<AuthStore>((set, get) => ({
        authorized: false,
        user: { email: '' , name:'', companyName:"" , isActive :true},
        setAuthorized: (authorized: boolean) => set({ authorized }),
        setUserAsync: async (user: UserDTO) => {
            await new Promise((resolve) => setTimeout(resolve, 1000))
set({ user })
        } ,
    }), {
        name: 'auth-storage', // Name for local storage key
    })
);

export default useAuthStore;