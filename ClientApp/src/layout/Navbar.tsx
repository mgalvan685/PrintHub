import { NavLink } from "react-router-dom";

export default function Navbar() {
  return (
    <nav className="bg-tactical-bg2 border-b border-tactical-border px-6 py-3 flex items-center gap-8">
      <div className="text-tactical-text text-xl font-bold tracking-wide">
        PrintHub
      </div>

      <NavLink
        to="/filaments"
        className={({ isActive }) =>
          `text-tactical-text2 hover:text-tactical-cyan ${
            isActive ? "text-tactical-cyan font-semibold" : ""
          }`
        }
      >
        Filaments
      </NavLink>

      <NavLink
        to="/materials"
        className={({ isActive }) =>
          `text-tactical-text2 hover:text-tactical-cyan ${
            isActive ? "text-tactical-cyan font-semibold" : ""
          }`
        }
      >
        Materials
      </NavLink>

      <NavLink
        to="/printers"
        className={({ isActive }) =>
          `text-tactical-text2 hover:text-tactical-cyan ${
            isActive ? "text-tactical-cyan font-semibold" : ""
          }`
        }
      >
        Printers
      </NavLink>

      <NavLink
        to="/projects"
        className={({ isActive }) =>
          `text-tactical-text2 hover:text-tactical-cyan ${
            isActive ? "text-tactical-cyan font-semibold" : ""
          }`
        }
      >
        Projects
      </NavLink>

      <NavLink
        to="/import"
        className={({ isActive }) =>
          `text-tactical-text2 hover:text-tactical-cyan ${
            isActive ? "text-tactical-cyan font-semibold" : ""
          }`
        }
      >
        Import
      </NavLink>
    </nav>
  );
}
