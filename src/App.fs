// Punto de entrada — orquestador de tabs + Programa Elmish
module DonPizzero.App

open Elmish
open Elmish.React
open Feliz
open DonPizzero.Types
open DonPizzero.State
open DonPizzero.Views

let private soles (m: decimal) = sprintf "S/ %.2f" (float m)

let private barraPedido (carrito: CartLine list) (dispatch: Msg -> unit) =
    Html.button [
        prop.className "fixed inset-x-4 bottom-20 z-20 flex items-center justify-between rounded-2xl bg-rosso px-5 py-3.5 font-extrabold text-white shadow-2xl transition active:bg-rosso-dark"
        prop.onClick (fun _ -> dispatch (IrATab TabPedido))
        prop.children [
            Html.span [ prop.text (sprintf "Ver pedido (%d)" (Cart.cantidadItems carrito)) ]
            Html.span [ prop.text (soles (Cart.total carrito)) ]
        ]
    ]

let private view (model: Model) (dispatch: Msg -> unit) =
    Html.div [
        prop.className "min-h-screen bg-crema pb-16"
        prop.children [
            match model.TabActiva with
            | TabInicio -> HomeView.view model dispatch
            | TabMenu ->
                Html.div [
                    prop.children [
                        Header.view model.Carrito dispatch
                        CategoryFilter.view model.CategoriaActiva dispatch
                        MenuList.view (menuFiltrado model) dispatch
                    ]
                ]
            | TabPedido -> CartView.view model.Carrito dispatch
            | TabPremios -> PremiosView.view model.Puntos model.Sellos
            | TabPerfil -> PerfilView.view model.Puntos dispatch

            if (model.TabActiva = TabInicio || model.TabActiva = TabMenu)
               && not (List.isEmpty model.Carrito) then
                barraPedido model.Carrito dispatch

            match model.Modal with
            | Some m -> ProductModal.view m dispatch
            | None -> Html.none

            match model.Selector with
            | Some modo -> LocalSelector.view modo dispatch
            | None -> Html.none

            TabBar.view model.TabActiva model.Carrito dispatch
        ]
    ]

#if DEBUG
open Elmish.HMR   // debe abrirse al final: intercepta Program.run para hot-reload
#endif

Program.mkProgram init update view
|> Program.withReactSynchronous "app"
|> Program.run
