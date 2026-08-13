import { useState } from "react";
import { usePrinters } from "../hooks/usePrinters";
import TacticalButton from "../components/TacticalButton";
import PrintersTable from "../components/printers/PrintersTable";
import AddPrinterModal from "../components/printers/AddPrinterModal";

export default function PrintersPage() {
  const { printers, addPrinter } = usePrinters();
  const [showModal, setShowModal] = useState(false);

  if (printers.isLoading) return <div>Loading...</div>;
  if (printers.isError) return <div>Error loading printers.</div>;

  return (
    <div className="flex flex-col gap-6">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Printers</h1>
        <TacticalButton onClick={() => setShowModal(true)}>
          Add Printer
        </TacticalButton>
      </div>

      <PrintersTable printers={printers.data} />

      {showModal && (
        <AddPrinterModal
          onClose={() => setShowModal(false)}
          onSubmit={(dto) => {
            addPrinter.mutate(dto);
            setShowModal(false);
          }}
        />
      )}
    </div>
  );
}
