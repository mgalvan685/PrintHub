export default function TacticalInput({
  label,
  value,
  onChange,
  type = "text"
}: {
  label: string;
  value: any;
  onChange: (v: any) => void;
  type?: string;
}) {
  return (
    <div className="flex flex-col gap-1 mb-3">
      <label className="text-tactical-text2">{label}</label>
      <input
        type={type}
        value={value}
        onChange={e => onChange(e.target.value)}
        className="bg-tactical-bg border border-tactical-border text-tactical-text px-3 py-2 rounded"
      />
    </div>
  );
}
