export default function TacticalButton({
  children,
  onClick
}: {
  children: React.ReactNode;
  onClick?: () => void;
}) {
  return (
    <button
      onClick={onClick}
      className="bg-tactical-cyan text-tactical-bg font-semibold px-4 py-2 rounded hover:bg-tactical-cyan2 transition"
    >
      {children}
    </button>
  );
}
