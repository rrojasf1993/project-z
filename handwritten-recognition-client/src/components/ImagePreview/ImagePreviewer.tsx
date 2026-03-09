
import type { ImagePreviewerProps } from "../../model/props/ImagePreviewerProps"

const ImagePreviewer=(props:ImagePreviewerProps)=>{
    return(<>
    <img src={props.imgSrc} alt={props.imgSrc} style={{overflow:"clip", maxWidth:"50%", maxHeight:"50%"}}/>
    </>)
}
export default ImagePreviewer;