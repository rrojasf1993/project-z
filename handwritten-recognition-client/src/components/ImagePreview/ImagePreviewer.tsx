
import type { IImagePreviewerProps } from "../../model/props/IImagePreviewerProps"

const ImagePreviewer=(props:IImagePreviewerProps)=>{
    return(<>
    <img src={props.imgSrc} alt={props.imgSrc}/>
    </>)
}
export default ImagePreviewer;