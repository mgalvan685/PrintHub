import Navbar from "./Navbar";

export default function Layout({ children }: { children: React.ReactNode }) {
  return (
    <div className="bg-tactical-bg min-h-screen text-tactical-text">
      <Navbar />
      <div className="p-6">{children}</div>
    </div>
  );
}
