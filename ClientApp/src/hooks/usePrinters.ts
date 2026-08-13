import { useQuery, useMutation } from "@tanstack/react-query";
import { getPrinters, createPrinter } from "../api/printers";
import type{ NewPrinterDto } from "../types/printer";

export const usePrinters = () => {
  const printers = useQuery({
    queryKey: ["printers"],
    queryFn: getPrinters
  });

  const addPrinter = useMutation({
    mutationFn: (dto: NewPrinterDto) => createPrinter(dto),
    onSuccess: () => printers.refetch()
  });

  return { printers, addPrinter };
};
