
import type { ImagePreviewerProps } from "../../model/props/ImagePreviewerProps"
import "./ImagePreviewer.css";

const ImagePreviewer=(props:ImagePreviewerProps)=>{
    return(<>
    <img src={props.imgSrc} alt={props.imgSrc} className="imagePreviewerImg"/>
    </>)
}
export default ImagePreviewer;