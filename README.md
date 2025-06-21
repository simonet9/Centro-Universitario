# Sistema de Gestión del Centro Deportivo Universitario

## 🎯 Objetivo General

Desarrollar un sistema completo para la **gestión de eventos deportivos e inscripciones (reservas)** en un centro universitario. Este sistema permite registrar personas, definir eventos deportivos y gestionar reservas, controlando el estado de asistencia de los participantes. Incluye además la **gestión de usuarios** con permisos específicos y un flujo de autenticación seguro.

---

## 🧱 Arquitectura y Estructura del Proyecto

La solución se organiza bajo el nombre `CentroEventos` y sigue los principios de **Arquitectura Limpia**, promoviendo la separación de responsabilidades y el desacoplamiento mediante inyección de dependencias.

### Estructura de Proyectos

- **CentroEventos.Aplicacion** (Librería de clases .NET 8)
  - Núcleo de la lógica de negocio.
  - Sin dependencias externas de la solución.
  - Incluye entidades (`Persona`, `EventoDeportivo`, `Reserva`, `Usuario`), validadores, repositorios (interfaces), casos de uso y excepciones personalizadas.

- **CentroEventos.Repositorios** (Librería de clases .NET 8)
  - Implementa la persistencia con **Entity Framework Core** y base de datos **SQLite** (modelo code first).
  - Contiene los repositorios concretos y la implementación de `ServicioAutorizacion`.
  - Referencia a `CentroEventos.Aplicacion`.

- **CentroEventos.UI** (Aplicación Blazor .NET 8)
  - Interfaz de usuario moderna y accesible.
  - Permite gestión de usuarios, eventos, reservas y personas.
  - Referencia a ambos proyectos anteriores.

---

## 📦 Organización del Repositorio

El repositorio se estructura para reflejar la arquitectura limpia y la división modular. Los proyectos suelen estar organizados en carpetas separadas, cada uno con su propia lógica y responsabilidades.

---

## 📌 Entidades Principales

- **Persona:** Id, DNI, Nombre, Apellido, Email, Teléfono.
- **EventoDeportivo:** Id, Nombre, Descripción, Fecha/Hora de inicio, Duración, Cupo máximo, Responsable.
- **Reserva:** Id, Persona, Evento, Fecha de alta, Estado de asistencia.
- **Usuario:** Id, Nombre, Apellido, Email, Hash de contraseña, Permisos.

---

## 📜 Reglas de Negocio

1. Un evento no puede exceder su cupo máximo de reservas.
2. Una persona no puede reservar dos veces el mismo evento.
3. No se pueden modificar eventos pasados.
4. Las fechas de inicio no pueden ser anteriores al presente.
5. No se puede eliminar un evento con reservas asociadas.
6. No se puede eliminar una persona responsable de eventos o con reservas.
7. El primer usuario registrado es Administrador (todos los permisos).
8. Los usuarios nuevos solo tienen permisos de lectura.

---

## ✅ Validaciones

Cada entidad tiene validadores específicos en `CentroEventos.Aplicacion`, asegurando unicidad y obligatoriedad de campos clave, así como la integridad de referencias y restricciones de negocio.

---

## ⚙️ Casos de Uso

Incluye CRUD completos para todas las entidades y casos específicos como:

- Alta de reservas con verificación de cupo y duplicidad.
- Listado de eventos con cupo disponible.
- Listado de asistentes a eventos pasados.

---

## 🔐 Autenticación y Permisos

- Enum `Permiso` define los permisos disponibles para usuarios sobre las distintas entidades.
- Servicio de autorización centraliza la verificación de permisos.
- El flujo de autenticación asegura el hash seguro de contraseñas y la asignación de roles adecuada.

---

## 🖥 Interfaz de Usuario (MudBlazor)

- Registro e inicio de sesión.
- Gestión visual de eventos, personas, reservas y usuarios (solo para autorizados).
- Visualización intuitiva de cupos y estado de asistencia.

---

## 🧪 Seguridad

- Contraseñas almacenadas únicamente como hash seguro.
- Sin almacenamiento de contraseñas originales ni recuperación automática.
- Validación estricta de autenticidad mediante comparación de hashes.

---
