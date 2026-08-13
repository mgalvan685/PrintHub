import { api } from "./axios";
import type { NewPrinterDto } from "../types/printer";

export const getPrinters = () =>
  api.get("/printers").then(r => r.data);

export const createPrinter = (dto: NewPrinterDto) =>
  api.post("/printers", dto).then(r => r.data);
