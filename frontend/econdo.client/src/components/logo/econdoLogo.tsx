import { ECondoLogoRounded } from "./econdoLogoRounded";
import { ECondoLogoText } from "./econdoLogoText";
import { LogoProps } from "./logoProps";

export interface ECondoLogoProps extends LogoProps {
  type?: 'mark' | 'full';
}

export function ECondoLogo({ type, ...others }: ECondoLogoProps) {
  if(type === 'mark')
    return <ECondoLogoRounded {...others} />

  return <ECondoLogoText {...others} />
}