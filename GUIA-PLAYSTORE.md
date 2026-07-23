# Don Pizzero — Guía: de la web a Google Play

## 1. Compilar
```bash
npm install        # instala vite-plugin-pwa (nuevo)
npm run build      # genera dist/ (PWA completa: manifest + service worker + iconos)
```

## 2. Publicar en HTTPS (requisito de Play Store)
Opción simple — Netlify Drop: entra a https://app.netlify.com/drop y arrastra la carpeta `dist/`.
Opción CLI — Vercel:
```bash
npx vercel login
npx vercel --prod
```
Resultado: una URL tipo `https://don-pizzero.vercel.app`. Verifica en el celular que
Chrome ofrezca "Agregar a pantalla de inicio" (señal de PWA válida).

## 3. Generar el paquete Android (TWA, sin Android Studio)
1. Entra a https://www.pwabuilder.com y pega tu URL.
2. Revisa el reporte (manifest y service worker deben salir en verde).
3. "Package for stores" → Android → descarga el `.aab`.
4. **Guarda el archivo de firma (keystore) que te genera: sin él no podrás actualizar la app.**
5. Descarga también el `assetlinks.json`.

## 4. Verificación de dominio
Sube `assetlinks.json` a tu hosting en la ruta exacta:
`https://TU-URL/.well-known/assetlinks.json`
(En este proyecto: crea `public/.well-known/` y pon el archivo ahí; recompila y redespliega.)
Sin esto, la app abre con la barra del navegador visible.

## 5. Google Play Console
1. Cuenta de desarrollador: https://play.google.com/console (pago único de US$ 25).
2. Crear app → subir el `.aab` primero a "Prueba interna", probarlo, luego promover a Producción.
3. Ficha de la tienda: nombre, descripción, capturas del celular (usa la app real),
   ícono 512px (`public/icon-512.png`) y una URL de política de privacidad
   (puede ser una página simple; pídemela y la genero).

## Notas
- Al ser TWA, cada mejora de la web se refleja en la app instalada SIN volver a subir el `.aab`.
- Solo se re-empaqueta si cambian nombre, iconos o el manifest.
- Antes de publicar: reemplazar los horarios TODO de `src/Data.fs`.
