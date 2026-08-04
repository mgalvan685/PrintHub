import { api } from "./axios";
import type { NewFilamentDto } from "../types/filament";

export const getFilaments = () =>
  api.get("/filaments").then(r => r.data);

export const createFilament = (dto: NewFilamentDto) =>
  api.post("/filaments", dto).then(r => r.data);
