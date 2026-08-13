export default function MaterialsTable({ materials }: { materials: any[] }) {
  if (!materials) return <div>No materials found.</div>;

  return (
    <table className="w-full table-fixed border border-tactical-border bg-tactical-bg2">
      <thead className="bg-tactical-bg">
        <tr>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Name</th>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Initial Cost</th>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Units</th>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Total Material</th>
          <th className="p-3 w-1/5 text-left text-tactical-text2 border-b border-tactical-border">Cost / Unit</th>
        </tr>
      </thead>

      <tbody>
        {materials.map(m => (
          <tr key={m.id} className="hover:bg-tactical-bg transition">
            <td className="p-3 text-left border-b border-tactical-border">{m.name}</td>
            <td className="p-3 text-left border-b border-tactical-border">${m.initial_Cost}</td>
            <td className="p-3 text-left border-b border-tactical-border">{m.units}</td>
            <td className="p-3 text-left border-b border-tactical-border">{m.total_Material}</td>
            <td className="p-3 text-left border-b border-tactical-border">${m.cost_Per_Unit}</td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
