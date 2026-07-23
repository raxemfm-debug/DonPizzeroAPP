// HITO 6.1 — Estado Elmish: tabs, búsqueda, carrito, Club y selector de local
module DonPizzero.State

open Elmish
open Fable.Core
open Browser.Dom
open DonPizzero.Types

// ── Modelo ───────────────────────────────────────────────────
type Tab =
    | TabInicio
    | TabMenu
    | TabPedido
    | TabPremios
    | TabPerfil

/// Modo del bottom-sheet de locales
type ModoSelector =
    | ParaEntrega          // guarda el local preferido en la app
    | ParaReserva          // al elegir, deriva a WhatsApp con la reserva

type ModalState = {
    Item: MenuItem
    Tamano: Size option
    Extras: Extra list
    Cantidad: int
}

type Model = {
    TabActiva: Tab
    CategoriaActiva: Category option
    Busqueda: string
    Modal: ModalState option
    Selector: ModoSelector option      // None = cerrado
    LocalEntrega: string option        // local elegido para entrega/recojo
    Carrito: CartLine list
    SiguienteLineaId: int
    Puntos: int
    Sellos: int
}

type Msg =
    | IrATab of Tab
    | SeleccionarCategoria of Category option
    | Buscar of string
    | AbrirModal of MenuItem
    | CerrarModal
    | ElegirTamano of Size
    | AlternarExtra of Extra
    | CambiarCantidadModal of int
    | ConfirmarAgregar
    | QuitarLinea of int
    | CambiarCantidadLinea of int * int
    | AbrirSelectorLocal of ModoSelector
    | CerrarSelectorLocal
    | ElegirLocal of Local
    | EnviarPorWhatsApp

// ── Helpers ──────────────────────────────────────────────────
let private clampCantidad n = max 1 (min 20 n)

let private modalInicial (item: MenuItem) = {
    Item = item
    Tamano =
        match item.Precio with
        | PorTamano _ -> Some Mediana
        | Fijo _ -> None
    Extras = []
    Cantidad = 1
}

let menuFiltrado (model: Model) =
    match model.CategoriaActiva with
    | None -> Data.menu
    | Some cat -> Data.menu |> List.filter (fun i -> i.Categoria = cat)

let resultadosBusqueda (model: Model) =
    let q = model.Busqueda.Trim().ToLower()
    if q = "" then []
    else
        Data.menu
        |> List.filter (fun i ->
            i.Nombre.ToLower().Contains q || i.Descripcion.ToLower().Contains q)

let nivel (puntos: int) =
    if puntos >= 300 then "Sobrino Estrella ⭐"
    elif puntos >= 100 then "Sobrino Fiel 🍕"
    else "Sobrino Nuevo 👋"

// ── Persistencia local (demo hasta la FASE 2) ────────────────
let private leerInt (clave: string) =
    let v = Browser.WebStorage.localStorage.getItem clave
    if isNull v then 0
    else
        match System.Int32.TryParse v with
        | true, n -> n
        | _ -> 0

let private leerStr (clave: string) =
    let v = Browser.WebStorage.localStorage.getItem clave
    if isNull v || v = "" then None else Some v

let private guardar (clave: string) (valor: string) =
    Browser.WebStorage.localStorage.setItem (clave, valor)

// ── Textos y URLs de WhatsApp ────────────────────────────────
let private soles (monto: decimal) = sprintf "S/ %.2f" (float monto)

let private descripcionLinea (l: CartLine) =
    let tamano =
        match l.Tamano with
        | Some t -> sprintf " (%s)" (Size.etiqueta t)
        | None -> ""
    let extras =
        match l.Extras with
        | [] -> ""
        | xs ->
            xs
            |> List.map (fun e -> e.Nombre)
            |> String.concat ", "
            |> sprintf "\n   + %s"
    sprintf "- %dx %s%s%s\n   %s"
        l.Cantidad l.Item.Nombre tamano extras (soles (CartLine.precioLinea l))

let textoPedido (localEntrega: string option) (lineas: CartLine list) =
    [ sprintf "🍕 *PEDIDO - %s %s*" (Data.negocio.Nombre.ToUpper()) (Data.negocio.Ciudad.ToUpper())
      ""
      lineas |> List.map descripcionLinea |> String.concat "\n"
      ""
      sprintf "*TOTAL: %s*" (soles (Cart.total lineas))
      (if Data.negocio.DeliveryGratis then "🛵 Delivery GRATIS" else "")
      sprintf "📍 Local: %s" (localEntrega |> Option.defaultValue "por confirmar")
      ""
      "Mis datos:"
      "Nombre: "
      "Dirección: "
      "Referencia: " ]
    |> String.concat "\n"

let urlWhatsApp (localEntrega: string option) (lineas: CartLine list) =
    sprintf "https://wa.me/%s?text=%s"
        Data.negocio.WhatsApp
        (JS.encodeURIComponent (textoPedido localEntrega lineas))

let urlReserva (local: string) =
    sprintf "https://wa.me/%s?text=%s"
        Data.negocio.WhatsApp
        (JS.encodeURIComponent (sprintf "Hola 👋, quiero reservar una mesa en Don Pizzero — %s" local))

// ── Init ─────────────────────────────────────────────────────
let init () : Model * Cmd<Msg> =
    { TabActiva = TabInicio
      CategoriaActiva = None
      Busqueda = ""
      Modal = None
      Selector = None
      LocalEntrega = leerStr "dp-local"
      Carrito = []
      SiguienteLineaId = 1
      Puntos = leerInt "dp-puntos"
      Sellos = leerInt "dp-sellos" },
    Cmd.none

// ── Update ───────────────────────────────────────────────────
let update (msg: Msg) (model: Model) : Model * Cmd<Msg> =
    match msg with
    | IrATab tab ->
        { model with TabActiva = tab }, Cmd.none

    | SeleccionarCategoria cat ->
        { model with CategoriaActiva = cat }, Cmd.none

    | Buscar texto ->
        { model with Busqueda = texto }, Cmd.none

    | AbrirModal item ->
        { model with Modal = Some (modalInicial item) }, Cmd.none

    | CerrarModal ->
        { model with Modal = None }, Cmd.none

    | ElegirTamano t ->
        { model with Modal = model.Modal |> Option.map (fun m -> { m with Tamano = Some t }) },
        Cmd.none

    | AlternarExtra extra ->
        let alternar (m: ModalState) =
            let extras =
                if m.Extras |> List.exists (fun e -> e.Id = extra.Id) then
                    m.Extras |> List.filter (fun e -> e.Id <> extra.Id)
                else
                    m.Extras @ [ extra ]
            { m with Extras = extras }
        { model with Modal = model.Modal |> Option.map alternar }, Cmd.none

    | CambiarCantidadModal delta ->
        { model with
            Modal =
                model.Modal
                |> Option.map (fun m -> { m with Cantidad = clampCantidad (m.Cantidad + delta) }) },
        Cmd.none

    | ConfirmarAgregar ->
        match model.Modal with
        | None -> model, Cmd.none
        | Some m ->
            let linea = {
                Id = model.SiguienteLineaId
                Item = m.Item
                Tamano = m.Tamano
                Extras = m.Extras
                Cantidad = m.Cantidad
            }
            { model with
                Carrito = model.Carrito @ [ linea ]
                SiguienteLineaId = model.SiguienteLineaId + 1
                Modal = None },
            Cmd.none

    | QuitarLinea id ->
        { model with Carrito = model.Carrito |> List.filter (fun l -> l.Id <> id) },
        Cmd.none

    | CambiarCantidadLinea (id, delta) ->
        let carrito =
            model.Carrito
            |> List.map (fun l ->
                if l.Id = id then { l with Cantidad = clampCantidad (l.Cantidad + delta) } else l)
        { model with Carrito = carrito }, Cmd.none

    | AbrirSelectorLocal modo ->
        { model with Selector = Some modo }, Cmd.none

    | CerrarSelectorLocal ->
        { model with Selector = None }, Cmd.none

    | ElegirLocal local ->
        match model.Selector with
        | Some ParaReserva ->
            let efecto (_: Dispatch<Msg>) =
                window.``open`` (urlReserva local.Nombre, "_blank") |> ignore
            { model with Selector = None }, Cmd.ofEffect efecto
        | Some ParaEntrega ->
            let efecto (_: Dispatch<Msg>) =
                guardar "dp-local" local.Nombre
            { model with LocalEntrega = Some local.Nombre; Selector = None },
            Cmd.ofEffect efecto
        | None -> model, Cmd.none

    | EnviarPorWhatsApp ->
        if List.isEmpty model.Carrito then
            model, Cmd.none
        else
            let puntosNuevos = model.Puntos + int (Cart.total model.Carrito)
            let sellosNuevos = if model.Sellos >= 10 then 1 else model.Sellos + 1
            let efecto (_: Dispatch<Msg>) =
                guardar "dp-puntos" (string puntosNuevos)
                guardar "dp-sellos" (string sellosNuevos)
                window.``open`` (urlWhatsApp model.LocalEntrega model.Carrito, "_blank") |> ignore
            { model with Puntos = puntosNuevos; Sellos = sellosNuevos },
            Cmd.ofEffect efecto
