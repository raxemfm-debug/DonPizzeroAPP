// HITO 6 — Barra de navegación inferior (5 tabs, estilo prototipo)
module DonPizzero.Views.TabBar

open Feliz
open DonPizzero.Types
open DonPizzero.State

let private item (activa: Tab) (tab: Tab) (emoji: string) (etiqueta: string) (badge: int) (dispatch: Msg -> unit) =
    let esActiva = activa = tab
    Html.button [
        prop.className (
            "relative flex flex-col items-center justify-center gap-0.5 transition "
            + (if esActiva then "text-rosso" else "text-carbon/50"))
        prop.onClick (fun _ -> dispatch (IrATab tab))
        prop.children [
            Html.span [ prop.className "text-xl"; prop.text emoji ]
            Html.span [ prop.className "text-[10px] font-extrabold"; prop.text etiqueta ]
            if badge > 0 then
                Html.span [
                    prop.className "absolute right-3 top-1 flex h-4 w-4 items-center justify-center rounded-full bg-rosso text-[9px] font-bold text-white"
                    prop.text (string badge)
                ]
        ]
    ]

let view (activa: Tab) (carrito: CartLine list) (dispatch: Msg -> unit) =
    Html.nav [
        prop.className "fixed inset-x-0 bottom-0 z-30 grid h-16 grid-cols-5 border-t border-carbon/10 bg-white"
        prop.children [
            item activa TabInicio "🏠" "Inicio" 0 dispatch
            item activa TabMenu "📋" "Menú" 0 dispatch
            item activa TabPedido "🛒" "Pedido" (Cart.cantidadItems carrito) dispatch
            item activa TabPremios "🎁" "Premios" 0 dispatch
            item activa TabPerfil "👤" "Perfil" 0 dispatch
        ]
    ]
