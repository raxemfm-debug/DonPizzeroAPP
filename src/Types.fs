// HITO 6 — Dominio de Don Pizzero Ayacucho (rediseño Prototipo-Plus)
module DonPizzero.Types

// ── Menú ─────────────────────────────────────────────────────
type Category =
    | Clasicas
    | Especiales
    | Criollas
    | Pastas
    | Promociones

type Size =
    | Personal
    | Mediana
    | Familiar

type Extra = {
    Id: string
    Nombre: string
    Precio: decimal
}

/// Fijo: pastas y promos · PorTamano: pizzas
type Pricing =
    | Fijo of decimal
    | PorTamano of Map<Size, decimal>

type MenuItem = {
    Id: string
    Nombre: string
    Descripcion: string
    Emoji: string             // icono de producto (estilo del prototipo)
    Categoria: Category
    Precio: Pricing
    ExtrasDisponibles: Extra list
    Etiqueta: string option
    Popular: bool             // aparece en "Las más pedidas"
}

// ── Marca ────────────────────────────────────────────────────
type Horario = {
    Dias: string
    Apertura: string
    Cierre: string
}

type Local = {
    Nombre: string
    Maps: string          // enlace a Google Maps
}

type Negocio = {
    Nombre: string
    Ciudad: string
    WhatsApp: string          // formato wa.me: 51XXXXXXXXX, sin '+'
    Horarios: Horario list
    DeliveryGratis: bool
    Facebook: string
    TikTok: string
    Locales: Local list
}

// ── Carrito ──────────────────────────────────────────────────
type CartLine = {
    Id: int
    Item: MenuItem
    Tamano: Size option
    Extras: Extra list
    Cantidad: int
}

// ── Helpers de dominio ───────────────────────────────────────
module Category =
    let todas = [ Clasicas; Especiales; Criollas; Pastas; Promociones ]

    let etiqueta =
        function
        | Clasicas -> "Clásicas"
        | Especiales -> "Especiales"
        | Criollas -> "Criollas"
        | Pastas -> "Pastas"
        | Promociones -> "Promos"

    let emoji =
        function
        | Clasicas -> "🍕"
        | Especiales -> "⭐"
        | Criollas -> "🌶️"
        | Pastas -> "🍝"
        | Promociones -> "🎉"

module Size =
    let todos = [ Personal; Mediana; Familiar ]

    let etiqueta =
        function
        | Personal -> "Personal"
        | Mediana -> "Mediana"
        | Familiar -> "Familiar"

module Pricing =
    /// Precio "desde" para las tarjetas del menú
    let desde =
        function
        | Fijo p -> p
        | PorTamano precios -> precios |> Map.toList |> List.map snd |> List.min

module CartLine =
    let precioBase (linea: CartLine) =
        match linea.Item.Precio, linea.Tamano with
        | Fijo p, _ -> p
        | PorTamano precios, Some t ->
            precios
            |> Map.tryFind t
            |> Option.defaultValue (Pricing.desde linea.Item.Precio)
        | PorTamano _, None -> Pricing.desde linea.Item.Precio

    let precioExtras (linea: CartLine) =
        linea.Extras |> List.sumBy (fun e -> e.Precio)

    let precioUnitario (linea: CartLine) =
        precioBase linea + precioExtras linea

    let precioLinea (linea: CartLine) =
        precioUnitario linea * decimal linea.Cantidad

module Cart =
    let total (lineas: CartLine list) =
        lineas |> List.sumBy CartLine.precioLinea

    let cantidadItems (lineas: CartLine list) =
        lineas |> List.sumBy (fun l -> l.Cantidad)
