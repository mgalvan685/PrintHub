export default function PrintersTable({ printers }: { printers: any[] }) {
  if (!printers) return <div>No printers found.</div>;

  return (
    <table className="w-full table-fixed border border-tactical-border bg-tactical-bg2">
      <thead className="bg-tactical-bg">
        <tr>
          <th className="p-3 w-1/4 text-left text-tactical-text2 border-b border-tactical-border">Brand</th>
          <th className="p-3 w-1/4 text-left text-tactical-text2 border-b border-tactical-border">Type</th>
          <th className="p-3 w-1/4 text-left text-tactical-text2 border-b border-tactical-border">Name</th>
          <th className="p-3 w-1/4 text-left text-tactical-text2 border-b border-tactical-border">Power / Hour (kWh)</th>
        </tr>
      </thead>

      <tbody>
        {printers.map(p => (
          <tr key={p.id} className="hover:bg-tactical-bg transition">
            <td className="p-3 text-left border-b border-tactical-border">{p.brand}</td>
            <td className="p-3 text-left border-b border-tactical-border">{p.type}</td>
            <td className="p-3 text-left border-b border-tactical-border">{p.name}</td>
            <td className="p-3 text-left border-b border-tactical-border">{p.power_Per_Hour}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
