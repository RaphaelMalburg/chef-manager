import hero from '../assets/hero.png';

function Hero() {
  return (
      <div className="hero min-h-screen" style={{ backgroundImage: `url(${hero})` }}>
          <div className="hero-overlay bg-opacity-60"></div>
          <div className="hero-content text-center text-neutral-content">
              <div className="max-w-lg">
                  <h1 className="mb-5 text-5xl font-bold">Take Control of Your Kitchen. Empower Your Chefs.</h1>
                  <p className="mb-5">Introducing Chema, the all-in-one kitchen management software designed to streamline operations, empower your team, and boost profits.</p>
                  <button className="btn btn-primary">Get Started</button>
              </div>
          </div>
      </div>
  );
}

export default Hero;