export default function TacticalCard({ children }: { children: React.ReactNode }) {
  return (
    <div className="bg-tactical-bg2 border border-tactical-border rounded-md p-4">
      {children}
    </div>
  );
}
