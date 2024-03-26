import AuthorizeView, { AuthorizedUser } from "../components/AuthorizeView";
import Hero from "../components/Hero";
import LogoutLink from "../components/LogoutLink";


function Home() {
  return (
      /**      <AuthorizeView>
          <span><LogoutLink>Logout <AuthorizedUser value="email" /></LogoutLink></span>
    
      </AuthorizeView> */
      <>
          <Hero />
      </>
  );
}

export default Home;