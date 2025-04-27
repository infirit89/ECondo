'use client';

import HeroSection from '@/components/heroSection';
import ComponentsSection from '@/components/componentsSection';
import UserRoleSection from '@/components/userRoleSection';
import TestimonialSection from '@/components/testimonialSection';
import CallToActionSection from '@/components/callToActionSection';

export default function HomePage() {
  return (
    <>
      <HeroSection/>
      <ComponentsSection/>
      <UserRoleSection/>
      <TestimonialSection/>
      <CallToActionSection/>
    </>
  );
}
