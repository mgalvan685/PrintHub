export default function FilamentsTable({ filaments }: { filaments: any[] }) {
  if (!filaments) return <div>No filaments found.</div>;

  return (
    <table className="w-full table-fixed border border-tactical-border bg-tactical-bg2">
      <thead className="bg-tactical-bg">
        <tr>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Brand</th>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Material</th>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Texture</th>
          <th className="p-1/5 text-left text-tactical-text2 border-b border-tactical-border">Color</th>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Cost/KG</th>
        </tr>
      </thead>

      <tbody>
        {filaments.map(f => (
          <tr key={f.id} className="hover:bg-tactical-bg transition">
            <td className="p-3 text-left border-b border-tactical-border">{f.brand}</td>
            <td className="p-3 text-left border-b border-tactical-border">{f.material}</td>
            <td className="p-3 text-left border-b border-tactical-border">{f.texture}</td>
            <td className="p-3 text-left border-b border-tactical-border">{f.color}</td>
            <td className="p-3 text-left border-b border-tactical-border">${f.cost}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
